using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningLight : MonoBehaviour
{
    public float startSpeed;
    public float startLimit;

    public float speed;
    public Vector2 lightLimit;

    private float add = 1f;
    private SpriteRenderer render;

    public bool isStarted = false;
    public bool isBlinking = false;

    [SerializeField] private float alpha;

    private void Start()
    {
        render = GetComponent<SpriteRenderer>();
        alpha = 0;
        render.color = new Color(render.color.r, render.color.g, render.color.b, alpha);
    }
    private void Update()
    {
        if(isStarted)
        {
            FadeIn();
        }

        if(isBlinking)
        {
            Blink();
        }
    }

    public void FadeIn()
    {
        if (alpha > startLimit)
        {
            isBlinking = true;
            return; 
        }

        alpha += Time.deltaTime * startSpeed;
        render.color = new Color(render.color.r, render.color.g, render.color.b, alpha);
    }

    public void Blink()
    {
        isStarted = false;

        alpha += Time.deltaTime * speed * add;

        if (alpha >= lightLimit.y)
        {
            add = -1;
        }
        else if (alpha <= lightLimit.x)
        {
            add = 1;
        }

        render.color = new Color(render.color.r, render.color.g, render.color.b, alpha);
    }
}
