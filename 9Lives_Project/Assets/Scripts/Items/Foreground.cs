using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foreground : MonoBehaviour
{
    public float speed = 1;

    private Transform Camera;
    private Vector3 originPos;
    private float newPosY;

    // Start is called before the first frame update
    void Start()
    {
        originPos = transform.position;
        Camera = LevelManager.instance.PlayingCamera;
    }

    // Update is called once per frame
    void Update()
    {
        if (Camera.position.y < originPos.y - 15f || Camera.position.y > originPos.y + 15f)
        {
            transform.position = originPos;
            return;
        }

        newPosY =Mathf.Lerp(newPosY, (Camera.position.y - originPos.y + 15f) / 10f * speed, Time.deltaTime * 20f);

        transform.position = originPos + Vector3.down * newPosY;
    }
}
