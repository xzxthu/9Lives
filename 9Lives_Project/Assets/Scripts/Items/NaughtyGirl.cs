using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NaughtyGirl : MonoBehaviour
{
    public float speed;
    public float turnLimit = 7f;
    public float timeIntervel = 3f;

    private float timeToShoot;
    private float timer;
    private bool needToShoot;

    private Rigidbody2D rigid;
    private bool isRight = true;

    private Vector2 dir;
    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.right * speed;
        timeToShoot = timeIntervel;
    }

    private void Update()
    {
        if(transform.position.x > turnLimit )
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            rigid.velocity = Vector2.right * speed * (-1);
        }
        else if(transform.position.x < -turnLimit)
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
            rigid.velocity = Vector2.right * speed;
        }


        if (timer < timeToShoot)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
            timeToShoot = Random.Range(1, 15) / 10 * timeIntervel;
            dir = (rigid.velocity.normalized+Vector2.up).normalized;
            BulletManager.GetInstance().ShootBullet(transform.position, dir, 9f, BulletType.takoyaki, 3f);
        }
        

        
    }
}
