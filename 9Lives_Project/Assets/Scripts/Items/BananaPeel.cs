using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BananaPeel : MonoBehaviour
{
    public float speed = 9f;
    [SerializeField] Sprite[] Images;
    public int imageNum = 0;

    private Animator anim;
    private Rigidbody2D rigid;
    private SpriteRenderer sprRndr;
    private int BananaTrigger = Animator.StringToHash("Banana");

    private bool isSlipping = false;
    private float timer = 0f;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        sprRndr = GetComponent<SpriteRenderer>();
        sprRndr.sprite = Images[imageNum];
    }

    private void Update()
    {
        if(isSlipping)
        {
            timer += Time.deltaTime;
            if(timer>2)
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 dir = new Vector2(
            PlayerActor.instance.rigid.velocity.x < 0 ? 1f:-1f,
            0.3f
            );
        this.gameObject.layer = LayerMask.NameToLayer("IgnorePlayer");
        anim.SetTrigger(BananaTrigger);
        rigid.velocity = dir * speed;
        isSlipping = true;
    }



}
