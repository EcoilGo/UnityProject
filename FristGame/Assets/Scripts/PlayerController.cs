using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public Collider2D coll;
    public Collider2D DisColl;
    public Transform cellingCheck,groundCheck;
    public float speed;
    public float jumpForce;
    public LayerMask ground;
    public int cherry = 0;
    private bool isHurt; //默认为false
    private bool isJump;
    private bool isGround;
    private bool jumpPressed;
    public int jumpCount;

    public Text CherryNum;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        isGround = Physics2D.OverlapCircle(groundCheck.position, 0.1f, ground);
        if (Input.GetButtonDown("Jump")&&jumpCount>0)
        {
            jumpPressed = true;
        }
        Crouch();
        CherryNum.text = cherry.ToString();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!isHurt)
        {
            Movement();
            Jump();
        }
        SwitchAnim();
    }

    //移动
    void Movement()
    {
        float horizontalMove = Input.GetAxisRaw("Horizontal");

        //角色移动
        rb.velocity = new Vector2(horizontalMove * speed , rb.velocity.y);
        anim.SetFloat("runing", Mathf.Abs(horizontalMove));
        
        if(horizontalMove != 0)
        {
            transform.localScale = new Vector3(horizontalMove, 1, 1);
        }
    }

    void Jump()
    {
        if(isGround)
        {
            jumpCount = 2;
            isJump = false;
        }
        if(jumpPressed && isGround)
        {
            isJump = true;
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            anim.SetBool("jumping", true);
            jumpCount--;
            SoundMananger.instance.JumpAudio();
            jumpPressed = false;
        }
        else if(jumpPressed && jumpCount>0 && !isGround)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            anim.SetBool("jumping", true);
            jumpCount--;
            SoundMananger.instance.JumpAudio();
            jumpPressed = false;
        }
    }
    void Crouch()
    {
        if (!Physics2D.OverlapCircle(cellingCheck.position,0.2f,ground))
        {
            if (Input.GetButton("Crouch"))
            {
                anim.SetBool("crouching", true);
                DisColl.enabled = false;
                speed = 2;
            } else 
            {
                anim.SetBool("crouching", false);
                DisColl.enabled = true;
                speed = 5;
            }
        }
    }

    //切换动画
    void SwitchAnim()
    {
        if(rb.velocity.y<0.1f && !coll.IsTouchingLayers(ground))
        {
            anim.SetBool("falling", true);
        }
        if (anim.GetBool("jumping"))
        {
            if(rb.velocity.y<0)
            {
                anim.SetBool("jumping", false);
                anim.SetBool("falling", true);
            }
        }
        else if (isHurt)
        {
            anim.SetBool("hurt", true);
            anim.SetFloat("runing", 0);
            if (Mathf.Abs(rb.velocity.x) < 0.1f)
            {
                anim.SetBool("hurt", false);
                isHurt = false;
            }
        }
        else if(coll.IsTouchingLayers(ground))
        {
            anim.SetBool("falling", false);
        } 
    }


    //碰撞触发器
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //收集物品
        if (collision.tag == "Collection")
        {
            collision.GetComponent<Animator>().Play("isGot");
            SoundMananger.instance.CherryAudio();
        }
        //DeadLine
        if (collision.tag == "DeadLine")
        {
            SoundMananger.instance.StopAudio();
            Invoke("Restart", 2f);
        }
    }

    void Restart()
    {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);    
    }

    //消灭敌人
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.gameObject.GetComponent<Enemy>();
            if (anim.GetBool("falling"))
            {
                enemy.JumpOn();
                rb.velocity = new Vector2(rb.velocity.x, 5);
                anim.SetBool("jumping", true);
            } else if(transform.position.x<collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(-5, rb.velocity.y);
                SoundMananger.instance.HurtAudio();
                isHurt = true;
            } else if (transform.position.x > collision.gameObject.transform.position.x)
            {
                rb.velocity = new Vector2(5, rb.velocity.y);
                SoundMananger.instance.HurtAudio();
                isHurt = true;
            }
        }

    }

    public void CherryAdd()
    {
        cherry += 1;
    }
    public int GetCherryCount()
    {
        return cherry;
    }
}
