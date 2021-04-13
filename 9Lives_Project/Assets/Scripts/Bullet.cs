using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public BulletType bulletType = BulletType.flying;
    public float hurtForce = 4f;
    public float hurtTime = 0.5f;
    public bool isAutoShoot = false;
    public Vector2 autoDir;
    //public Sprite[] sprites;
    public GameObject[] bulletSkins;

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

        for(int i=0;i<bulletSkins.Length;i++)
        {
            bulletSkins[i].SetActive(false);
        }

        //bulletSkins[(int)bulletType].SetActive(false);

        /*try
        {
            render = GetComponent<SpriteRenderer>();
            render.enabled = false;
        }
        catch
        {

        }*/

    }

    private void Update()
    {

        if(needToShoot)
        {
            needToShoot = false;
            Shoot();
        }

        if (isAutoShoot) return;

        float distance = Vector3.Distance(transform.position, startPoint);
        if(distance>60)
        {
            isOutOfScreen = true;
            //render.enabled = false; //防止再开的时候闪现
            //Debug.Log((int)bulletType);
            bulletSkins[(int)bulletType].SetActive(false);
            
        }
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Bullet hit " + collision.gameObject.tag);
        if (collision.gameObject.tag == "Player")
        {
            PlayerActor.instance.isHurted = true;
            if(isAutoShoot)
            {
                PlayerActor.instance.hurtDir = autoDir.normalized;
            }
            else
            {
                PlayerActor.instance.hurtDir = direction.normalized;
            }
            
            PlayerActor.instance.hurtForce = hurtForce;
            PlayerActor.instance.hurtTime = hurtTime;
            PlayerActor.instance.isDown = true;//掉下去

            isHitting = true; //pool回收
            //render.enabled = false;
            bulletSkins[(int)bulletType].SetActive(false);
        }
    }

    private void Shoot()
    {
        transform.position = startPoint;
        //Debug.Log("startPoiont" + startPoint);
        rigid.velocity = direction * speed;
        //render.enabled = true;
        //render.sprite = sprites[(int)bulletType];
        bulletSkins[(int)bulletType].SetActive(true);
    }
}

public enum BulletType
{
    flying = 0,
    drop = 1,
    pizza = 2,
    brick = 3,
    takoyaki = 4,
    bird = 5,
    underwear = 6,
}