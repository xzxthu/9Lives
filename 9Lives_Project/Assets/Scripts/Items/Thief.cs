using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thief : MonoBehaviour
{
    private Dialog dialog;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        dialog = GetComponentInChildren<Dialog>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            anim.SetBool("isAttacking",true);
            dialog.AutoPlay(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            anim.SetBool("isAttacking", false);
            dialog.loopPlay = false;
        }
    }
}
