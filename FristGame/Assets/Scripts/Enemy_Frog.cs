using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Frog : Enemy
{
    private Rigidbody2D rb;
    //private Animator Anim;
    private Collider2D Coll;
    public LayerMask Ground;
    public Transform leftpoint, rightpoint;
    public bool Faceleft = true;
    public float Speed,JumpForce;
    private float leftx, rightx;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        //Anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        Coll = GetComponent<Collider2D>();
        transform.DetachChildren();
        leftx = leftpoint.position.x;
        rightx = rightpoint.position.x;
        Destroy(leftpoint.gameObject);
        Destroy(rightpoint.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        SwitchAnim();
    }

    void Movement()
    {
        if(Faceleft)
        {
            if (transform.position.x < leftx)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                Faceleft = false;
            }
            if (Coll.IsTouchingLayers(Ground)&&Faceleft)
            {
                Anim.SetBool("jumping", true);
                rb.velocity = new Vector2(-Speed, JumpForce);
            }
            if (Coll.IsTouchingLayers(Ground) && !Faceleft)
            {
                Anim.SetBool("jumping", true);
                rb.velocity = new Vector2(Speed, JumpForce);
            }
        }
        else
        {
            if (transform.position.x > rightx)
            {
                transform.localScale = new Vector3(1, 1, 1);
                Faceleft = true;
            }
            if (Coll.IsTouchingLayers(Ground)&& !Faceleft)
            {
                Anim.SetBool("jumping", true);
                rb.velocity = new Vector2(Speed, JumpForce);
            }
            if (Coll.IsTouchingLayers(Ground) && Faceleft)
            {
                Anim.SetBool("jumping", true);
                rb.velocity = new Vector2(-Speed, JumpForce);
            }
        }
    }

    void SwitchAnim()
    {
        if(Anim.GetBool("jumping"))
        {
            if(rb.velocity.y<0.1)
            {
                Anim.SetBool("jumping", false);
                Anim.SetBool("falling", true);
            }
        }
        if(Coll.IsTouchingLayers(Ground) && Anim.GetBool("falling"))
        {
            Anim.SetBool("falling", false);
        }
    }


}
