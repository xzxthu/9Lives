using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Argue : MonoBehaviour
{
    public Animator woman;
    public Animator man;

    private bool needShoot;
    private float timer;

    private void Start()
    {
        GetComponentInChildren<Dialog>().AutoPlay(true);
    }

    private void Update()
    {
        if(needShoot)
        {
            timer += Time.deltaTime;
        }

        if(timer>4f)
        {
            woman.SetBool("isPizza", true);
            man.SetBool("isPizza", true);
            timer = 0;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            woman.SetBool("isPizza", true);
            man.SetBool("isPizza", true);
            needShoot = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            woman.SetBool("isPizza", false);
            man.SetBool("isPizza", false);
            needShoot = false;
            timer = 0;
        }
    }

    public void PauseShoot()
    {
        woman.SetBool("isPizza", false);
        man.SetBool("isPizza", false);
    }
}
