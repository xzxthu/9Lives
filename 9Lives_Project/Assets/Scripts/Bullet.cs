using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    public float hurtForce = 4f;
    public float hurtTime = 0.5f;

    [HideInInspector] public bool isOutOfScreen;
    [HideInInspector] public bool isHitting;

    [HideInInspector] public Vector2 startPoint;
    [HideInInspector] public Vector2 direction;
    [HideInInspector] public float speed;

    [HideInInspector] public bool needToShoot = false;


    private Rigidbody2D rigid;
    private SpriteRenderer render;

    private void Start()
    {
        isOutOfScreen = false;
        rigid = GetComponent<Rigidbody2D>();
        render = GetComponent<SpriteRenderer>();
        
    }

    private void Update()
    {

        if(needToShoot)
        {
            needToShoot = false;
            Shoot();
        }

        float distance = Vector3.Distance(transform.position, startPoint);
        if(distance>20)
        {
            isOutOfScreen = true;
            render.enabled = false; //防止再开的时候闪现
        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Bullet hit " + collision.gameObject.tag);
        if (collision.gameObject.tag == "Player")
        {
            Level_2_Manager.instance.isHurted = true;
            Level_2_Manager.instance.hurtDir = direction.normalized;
            Level_2_Manager.instance.hurtForce = hurtForce;
            Level_2_Manager.instance.hurtTime = hurtTime;
            Level_2_Manager.instance.isDown = true;//掉下去

            isHitting = true; //pool回收
            render.enabled = false;
        }
    }

    private void Shoot()
    {
        transform.position = startPoint;
        rigid.velocity = direction * speed;
        render.enabled = true;
    }
}
