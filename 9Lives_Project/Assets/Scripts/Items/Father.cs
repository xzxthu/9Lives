using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Father : MonoBehaviour
{
    public Transform daughter;

    private float girlSpeed;
    private float speed;
    private float turnLimit;
    private float distanceToDau;

    private Rigidbody2D rigid;

    private void Start()
    {
        girlSpeed = daughter.GetComponent<NaughtyGirl>().speed;
        speed = girlSpeed;
        turnLimit = daughter.GetComponent<NaughtyGirl>().turnLimit;
        rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.right * girlSpeed;
        distanceToDau = Vector2.Distance(transform.position,daughter.position);
    }
    private void Update()
    {
        if(daughter.GetComponent<NaughtyGirl>().turned)
        {
            daughter.GetComponent<NaughtyGirl>().turned = false;
            //speed = 3 * girlSpeed;
        }

        if (transform.position.x > turnLimit)
        {
            speed = girlSpeed;
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            rigid.velocity = Vector2.right * speed * (-1);
        }
        else if (transform.position.x < -turnLimit)
        {
            speed = girlSpeed;
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            rigid.velocity = Vector2.right * speed;
        }
    }
}
