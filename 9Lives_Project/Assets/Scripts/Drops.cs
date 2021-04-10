using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drops : MonoBehaviour
{
    public GameObject drop;
    public GameObject dropCollider;

    [Header("time interval")]
    public float dropTime = 2f;

    private float timer = 0f;
    private bool isPlayingAnimation = false;

    private void Start()
    {
        drop.SetActive(false);
        dropCollider.SetActive(false);
    }

    private void Update()
    {
        if(!isPlayingAnimation)
            timer += Time.deltaTime;

        if(timer>dropTime)
        {
            drop.SetActive(true);
            dropCollider.SetActive(true);
            timer = 0f;
            isPlayingAnimation = true;
        }
    }

    public void PauseAnimation()
    {
        isPlayingAnimation = false;
        dropCollider.SetActive(false);
        drop.SetActive(false);
    }

}
