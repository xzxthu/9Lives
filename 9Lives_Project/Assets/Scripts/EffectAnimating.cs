using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectAnimating : MonoBehaviour
{
    [Header("Image Settings")]
    [SerializeField]
    Texture[] Images;

    [SerializeField]
    [Tooltip("Animation speed in FPS")]
    [Range(0, 60f)]
    float AnimationSpeed = 3f;

    RawImage ImageScript;
    float AnimTimer;
    int AnimIndex = 0;
    float AnimFrameTime;

    // Start is called before the first frame update
    void Start()
    {
        ImageScript = GetComponent<RawImage>();
        ImageScript.texture = Images[0];
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
                ImageScript.texture = Images[AnimIndex];
                AnimTimer = 0;
            }
        }
    }
}
