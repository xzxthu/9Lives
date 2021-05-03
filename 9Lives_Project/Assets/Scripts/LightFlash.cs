using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlash : MonoBehaviour
{
    [Range(0, 1)] public float flashRange = 1f;
    public float speed = 20f;
    [Range(1, 10)] public int frequenceRange = 1;

    private SpriteRenderer render;
    private float originAlpha;
    private float alpha;
    private float newAlpha;
    private float timer = 0f;

    private void Start()
    {
        render = GetComponent<SpriteRenderer>();
        originAlpha = render.color.a;
        SetAlpha(0.5f);
    }

    private void Update()
    {
        timer++;
        if(timer>frequenceRange)
        {
            newAlpha = originAlpha + (float)Random.Range(1, 10) / 10f * flashRange;
            timer = 0;
        }
        alpha = Mathf.Lerp(alpha, newAlpha, speed * Time.deltaTime);
        SetAlpha(alpha);
    }

    private void SetAlpha(float alpha)
    {
        render.color = new Color(render.color.r, render.color.g, render.color.b, alpha);
    }
}
