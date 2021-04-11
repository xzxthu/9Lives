using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignMan : MonoBehaviour
{
    public Vector2 ShoutPositionLimit = new Vector2(0,10);

    private Transform playerPos;
    private Animator anim;

    private void Start()
    {
        playerPos = GameObject.FindWithTag("Player").transform;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (playerPos.position.y>ShoutPositionLimit.x && playerPos.position.y<ShoutPositionLimit.y)
        {
            anim.SetBool("isShout", true);
            
        }
        else
        {
            anim.SetBool("isShout", false);
        }
    }
}
