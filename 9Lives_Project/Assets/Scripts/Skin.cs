using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;

public class Skin : MonoBehaviour
{
    public List<SpriteResolver> spriteResolvers = new List<SpriteResolver>();
    public SpriteResolver headResolver;

    public SkinState nowSkin;
    private Dictionary<SkinState, SkinState> cribeSkin = new Dictionary<SkinState, SkinState>();

    private Animator anim;



    private void Start()
    {
        nowSkin = SkinState.fluffy_normal;

        Init();
        ChangeSkin(nowSkin);
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        ChangeCribeFace();
    }

    private void Init()
    {
        foreach (var resolver in FindObjectsOfType<SpriteResolver>())
        {
            spriteResolvers.Add(resolver);
        }

        cribeSkin[SkinState.fluffy_normal] = SkinState.fluffy_cribe;
    }

    private void ChangeSkin(SkinState newstate)
    {
        foreach (var resolver in spriteResolvers)
        {
            resolver.SetCategoryAndLabel(resolver.GetCategory(), newstate.ToString());
        }
    }

    private void ChangeCribeFace()
    {
        if (PlayerActor.instance.isHanging || anim.GetBool("isJumping"))
        {
            headResolver.SetCategoryAndLabel(headResolver.GetCategory(), cribeSkin[nowSkin].ToString());
        }
        else
        {
            headResolver.SetCategoryAndLabel(headResolver.GetCategory(), nowSkin.ToString());
        }
    }
}



public enum SkinState
{
    fluffy_normal,
    fluffy_cribe,
}