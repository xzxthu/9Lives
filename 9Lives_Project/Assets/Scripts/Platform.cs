using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public PlatformType platformType;

    public Sprite[] platformSprites;

    private SpriteRenderer render;

    private Transform playerDefTransform;

    private void Start()
    {
        render = GetComponent<SpriteRenderer>();
        render.sprite = platformSprites[(int)platformType];
        playerDefTransform = GameObject.FindWithTag("Player").transform.parent;
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (platformType != PlatformType.cable)
            return;

        if(collision.gameObject.tag=="Player")
        {
            collision.gameObject.transform.parent = gameObject.transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (platformType != PlatformType.cable)
            return;

        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.parent = playerDefTransform;
        }
    }
}

public enum PlatformType
{
    empty = 0,
    normal = 1,
    cable = 2,
}
