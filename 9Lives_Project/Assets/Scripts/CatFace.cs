using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatFace : MonoBehaviour
{
    public GameObject[] cats;
    [HideInInspector] public int nowFace = 0;

    private void Awake()
    {
        nowFace = 0;
        ChangeCatSkin(0);
    }

    public void ChangeCatSkin(int cat)
    {
        nowFace = cat;

        for (int i = 0; i < cats.Length; i++)
        {
            cats[i].SetActive(false);
        }

        cats[cat].SetActive(true);
    }

    public void SetFaceBool(string name, bool setbool)
    {
        cats[nowFace].GetComponent<Animator>().SetBool(name,setbool);
    }

    public void SetFaceTrigger(string name)
    {
        cats[nowFace].GetComponent<Animator>().SetTrigger(name);
    }
}

public enum CatExpression
{
    idle = 0,
    clibe = 1,
    running = 2,
    closeeyes = 3,
    lookup = 4,
    hurted = 5,
    
}
