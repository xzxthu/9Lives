using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignMan : MonoBehaviour
{
    public RoundTwo RoundTwo;

    private Animator anim;
    private Dialog dialog;
    private bool hasRoundTwo;

    private void Start()
    {
        anim = GetComponent<Animator>();
        dialog = GetComponentInChildren<Dialog>();
    }

    private void Update()
    {
        if(RoundTwo.hasFallDown && !hasRoundTwo)
        {
            hasRoundTwo = true;
            anim.SetBool("isShy", true);
            dialog.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player") && !RoundTwo.hasFallDown)
        {
            anim.SetBool("isShout", true);

            dialog.AutoPlay(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !RoundTwo.hasFallDown)
        {
            anim.SetBool("isShout", false);
            dialog.loopPlay = false;
        }
    }
}
