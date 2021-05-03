using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundTwo : MonoBehaviour
{
    public bool needToCloseInRoundTwo = false;
    public float ShowUpDistance = 10f;

    private Transform Camera;
    private SpriteRenderer render;
    private bool hasReached = false;
    [HideInInspector] public bool hasFallDown = false;

    private void Start()
    {
        Camera = LevelManager.instance.PlayingCamera;
        render = GetComponent<SpriteRenderer>();
        if( !(LevelManager.instance.TestMode || needToCloseInRoundTwo))
            render.enabled = false;
        hasReached = false;
        hasFallDown = false;
    }

    private void Update()
    {
        if (LevelManager.instance.TestMode)
            return;

        if ( Camera.position.y > transform.position.y + 7.5f)
        {
            hasReached = true;
        }

        if(hasReached && Camera.position.y < transform.position.y - ShowUpDistance)
        {
            hasFallDown = true;

            if (needToCloseInRoundTwo)
            {
                render.enabled = false;
                return;
            }

            render.enabled = true;
        }
    }
}
