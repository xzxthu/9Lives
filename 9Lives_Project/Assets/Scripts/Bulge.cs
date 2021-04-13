using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bulge : MonoBehaviour
{
    private SpriteRenderer render;

    private void Start()
    {
        render = GetComponent<SpriteRenderer>();
        render.enabled = false;
    }
}
