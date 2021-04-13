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

        if (LevelManager.instance.TestMode)
        {
            catAnimator.SetTrigger("wakeUpTrigger");
            EndingOfOpening();
            catFace.SetFaceBool("isClosingEyes", false);

        }
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

    private void TurnOffLights()
    {
        for (int i = 0; i < lights.Length; i++)
        {
            lights[i].isEnding = true;
        }
    }

    public void PlayDialog()
    {
        dialog.AutoPlay(false);
    }

    public void ChangeCatPos()
    {
        catAnimator.SetTrigger("wakeUpTrigger");
        catFace.SetFaceBool("isClosingEyes", false);
        catFace.SetFaceBool("isLookingUp", true);
    }

    public void EndingOfOpening()
    {
        LevelManager.instance.dontMove = false;
        catAnimator.SetBool("isOpening", false);
        catFace.SetFaceBool("isLookingUp", false);
        TurnOffLights();
    }

}
