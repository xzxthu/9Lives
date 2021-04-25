using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    public Transform center;

    private float radius;
    private float speed;
    private float nowAngle;

    private void Start()
    {
        radius = Vector2.Distance(center.position, transform.position);
        speed = 1.5f / 12f;
        nowAngle = Vector2.Angle((transform.position-center.position),Vector2.right);
        if (transform.position.y < center.position.y)
            nowAngle = 360f - nowAngle;

    }

    private void FixedUpdate()
    {
        nowAngle -= speed;
        float new_x = center.position.x + Mathf.Cos((nowAngle) / 180f * Mathf.PI) * radius;
        float new_y = center.position.y + Mathf.Sin((nowAngle) / 180f * Mathf.PI) * radius;

        //GetComponentInChildren<Rigidbody2D>().MovePosition(new Vector3(new_x, new_y, transform.position.z));

        transform.position = new Vector3(new_x, new_y, transform.position.z);

    }
}
