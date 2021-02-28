using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IKTest : MonoBehaviour
{
    public Transform testGround;
    public float MaxHeight;
    private Animator anim;
    private float height;

    private void Start()
    {
        anim = GetComponent<Animator>();

    }

    private void Update()
    {
        height = Mathf.Min(testGround.position.y, 1f);

        anim.SetFloat("Blend", height/MaxHeight);
    }

}
