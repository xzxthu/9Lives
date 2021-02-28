using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoofTop : MonoBehaviour
{
    public GameObject endingUI;
    public Transform cat;

    private void Update()
    {
        if(Vector3.Distance(cat.position,transform.position)<2f)
        {
            endingUI.SetActive(true);

            if (LevelManager.instance.isDrawMode) //绘画模式胜利
            {
                LevelManager.instance.DrawEnding();
                LevelManager.instance.Save();
            }
            else   //试玩胜利
            {

            }
        }
    }


    
}
