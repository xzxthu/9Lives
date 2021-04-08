using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundOneWay : MonoBehaviour
{
    [Range(1,200)]public int GroundNum;
    private bool onStand;
    private bool isColli;

    private void FixedUpdate()
    {
        

        if ((Input.GetAxis("Vertical") < -0.1f || PlayerActor.instance.isDown) && onStand)//One Way Down
        {
            Debug.Log(PlayerActor.instance.isDown);
            Debug.Log(Input.GetAxis("Vertical"));
            CloseGround();
            Invoke("RestoreGround", 0.5f);
        }

        if (isColli)
        {
            onStand = true;
        }
        else
        {
            onStand = false;


        }
    }

    private void CloseGround()//One way down 使用
    {
        GetComponent<BoxCollider2D>().enabled = false;
        gameObject.layer = LayerMask.NameToLayer("Default");
        Debug.Log("GroundClose!");
        
    }

    public void RestoreGround() //One way down 恢复使用
    {
        this.gameObject.layer = LayerMask.NameToLayer("Ground");
        GetComponent<BoxCollider2D>().enabled = true;
        PlayerActor.instance.isDown = false;
    }

    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            isColli = true;
            
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            isColli = false;
            PlayerActor.instance.isDown = false;
        }
    }
}
