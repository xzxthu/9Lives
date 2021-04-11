using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Argue : MonoBehaviour
{
    public Animator woman;
    public Animator man;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            woman.SetBool("isPizza", true);
            man.SetBool("isPizza", true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            woman.SetBool("isPizza", false);
            man.SetBool("isPizza", false);
        }
    }
}
