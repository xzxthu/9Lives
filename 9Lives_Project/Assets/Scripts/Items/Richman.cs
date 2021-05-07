using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Richman : MonoBehaviour
{
    private Dialog dialog;
    private Animator anim;

    private void Start()
    {
        try
        {
            dialog = GetComponentInChildren<Dialog>();
        }
        catch
        { }

        anim = GetComponent<Animator>();
    }
    private void Update()
    {

        

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            dialog.AutoPlay(true);
        }
    }



    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            dialog.loopPlay = false;
        }
    }
}
