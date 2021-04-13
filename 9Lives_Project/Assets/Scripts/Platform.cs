using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    private SpriteRenderer render;

    private void Start()
    {
        render = GetComponent<SpriteRenderer>();
        render.enabled = false;
    }
}
