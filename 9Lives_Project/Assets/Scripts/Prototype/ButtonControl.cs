using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonControl : MonoBehaviour
{
    public void EnterDraw()
    {
        LevelManager.instance.EnterDraw ();
    }

    public void EnterLoad()
    {
        LevelManager.instance.EnterLoad();
    }
}
