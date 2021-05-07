using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Workers : MonoBehaviour
{
    public Animator worker_1;
    public Animator worker_2;
    public Dialog dialog;
    public RoundTwo giraffe;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (giraffe.hasFallDown)
                return;
                worker_1.SetBool("isShout", true);
            worker_2.SetBool("isShout", true);
            dialog.AutoPlay(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (giraffe.hasFallDown)
                return;
            worker_1.SetBool("isShout", false);
            worker_2.SetBool("isShout", false);
            dialog.loopPlay = false;
        }
    }
}
