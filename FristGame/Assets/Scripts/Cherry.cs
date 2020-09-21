using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cherry : MonoBehaviour
{
    // Start is called before the first frame update
    public void Death()
    {
        FindObjectOfType<PlayerController>().CherryAdd();
        Destroy(gameObject);
    }
}
