using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    public GameObject[] needToHide;


    private void Start()
    {
        for(int i=0; i<needToHide.Length;i++)
        {
            needToHide[i].SetActive(false);
        }
    }

    private void Update()
    {

    }

}
