using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectAnimating : MonoBehaviour
{
    public bool playOneTime = false;

    [SerializeField] Sprite[] Images;

    [SerializeField, Tooltip("Animation speed in FPS"), Range(0, 60f)] 
    float AnimationSpeed = 3f;

    Image ImageScript;
    float AnimTimer;
    int AnimIndex = 0;
    float AnimFrameTime;

    // Start is called before the first frame update
    void Start()
    {
        ImageScript = GetComponent<Image>();
        ImageScript.sprite = Images[0];
        AnimFrameTime = 1.0f / AnimationSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Images.Length > 1)
        {
            AnimTimer += Time.deltaTime;
            if (AnimTimer >= AnimFrameTime)
            {
                AnimIndex = (AnimIndex + 1) % Images.Length;
                ImageScript.sprite = Images[AnimIndex];
                AnimTimer = 0;
                if(AnimIndex==Images.Length && playOneTime)
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}
