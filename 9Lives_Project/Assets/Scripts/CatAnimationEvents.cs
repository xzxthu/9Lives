using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAnimationEvents : MonoBehaviour
{
    public CatMoveController controller;

    public void ResetHunging()
    {

        Level_2_Manager.instance.isHanging = false;


    }

    
}
