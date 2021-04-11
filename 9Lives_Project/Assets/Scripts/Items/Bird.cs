using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    public float speed;
    public float turnLimit = 7f;

    private Rigidbody2D rigid;
    private Bullet bullet;
    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.left * speed;
        bullet = GetComponent<Bullet>();
    }

    private void Update()
    {
        if (transform.position.x > turnLimit)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            rigid.velocity = Vector2.left * speed;
            bullet.autoDir = Vector2.left;
        }
        else if (transform.position.x < -turnLimit)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            rigid.velocity = Vector2.right * speed;
            bullet.autoDir = Vector2.right;
        }
    }
}
