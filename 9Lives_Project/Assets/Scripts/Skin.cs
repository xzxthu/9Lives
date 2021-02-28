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
        }
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if(Level_2_Manager.instance.isHanging||anim.GetBool("isJumping"))
        {
            headResolver.SetCategoryAndLabel(headResolver.GetCategory(),"cribe");
        }
        else
        {
            headResolver.SetCategoryAndLabel(headResolver.GetCategory(), "normal");
        }
    }
}
