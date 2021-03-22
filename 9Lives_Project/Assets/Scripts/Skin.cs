using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;

public class Skin : MonoBehaviour
{
    public List<SpriteResolver> spriteResolvers = new List<SpriteResolver>();
    public SpriteResolver headResolver;

    private Animator anim;

    private void Start()
    {
        foreach(var resolver in FindObjectsOfType<SpriteResolver>())
        {
            spriteResolvers.Add(resolver);
            resolver.SetCategoryAndLabel(resolver.GetCategory(), "fluffy_normal");
        }


        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if(PlayerActor.instance.isHanging||anim.GetBool("isJumping"))
        {
            headResolver.SetCategoryAndLabel(headResolver.GetCategory(), "fluffy_cribe");
        }
        else
        {
            headResolver.SetCategoryAndLabel(headResolver.GetCategory(), "fluffy_normal");
        }
    }
}
