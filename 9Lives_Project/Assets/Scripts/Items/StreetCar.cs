using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetCar : MonoBehaviour
{
    public bool goToRight;
    public float speed = 0.2f;
    public float killDistance = 25;
    private int dir = 1;
    //private float timer = 0;
    private float distance;

    private Vector3 originPos;
    private Rigidbody2D rigid;

    private void Start()
    {
        if(goToRight)
        {
            transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z);
            dir *= -1;
        }

        originPos = transform.position;
        rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.left * speed * dir;
    }

    private void Update()
    {
        //transform.position = new Vector3(transform.position.x - speed * dir, transform.position.y, transform.position.z);
        //timer += Time.deltaTime;
        if(Mathf.Abs(rigid.velocity.x)<0.2f)
        {
            rigid.velocity = Vector2.left * speed * dir;
        }

        distance = Vector3.Distance(transform.position, originPos);
        if(distance>killDistance)
        {
            transform.position = originPos;
        }
    }

}
