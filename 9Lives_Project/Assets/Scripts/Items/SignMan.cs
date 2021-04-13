using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignMan : MonoBehaviour
{
    private Animator anim;
    private Dialog dialog;

    private void Start()
    {
        anim = GetComponent<Animator>();
        dialog = GetComponentInChildren<Dialog>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            anim.SetBool("isShout", true);

            dialog.AutoPlay(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            anim.SetBool("isShout", false);
            dialog.loopPlay = false;
        }
    }
}
