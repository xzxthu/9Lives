using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public bool isOutOfScreen;

    [HideInInspector] public Vector2 startPoint;
    [HideInInspector] public Vector2 direction;
    [HideInInspector] public float speed;
    

    private Rigidbody2D rigid;

    private void Start()
    {
        isOutOfScreen = false;
        rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = direction.normalized * speed;
        transform.position = startPoint;
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, startPoint);
        if(distance>10)
        {
            isOutOfScreen = true;
        }
    }

}
