using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opening : MonoBehaviour
{
    public Animator catAnimator;
    public CatFace catFace;
    public OpeningLight[] lights;

    public Dialog dialog;

    private void Start()
    {
        catAnimator.SetBool("isOpening", true);
        catFace.SetFaceBool("isClosingEyes", true);

        //dialog.AutoPlay(false);
    }

    private void Update()
    {

    }

    public void TurnOnLights()
    {
        for(int i=0;i<lights.Length;i++)
        {
            lights[i].isStarted = true;
        }
    }

    public void StartLightBlink()
    {
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].isBlinking = true;
        }
    }

    public void PlayDialog()
    {
        dialog.AutoPlay(false);
    }

    

}
