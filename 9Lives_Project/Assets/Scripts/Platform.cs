using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public PlatformType platformType;

    public Sprite[] platformSprites;

    private SpriteRenderer render;

    private void Start()
    {
        render = GetComponent<SpriteRenderer>();
        render.sprite = platformSprites[(int)platformType];
    }
}

public enum PlatformType
{
    empty = 0,
    normal = 1,
}
