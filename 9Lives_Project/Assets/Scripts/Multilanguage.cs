using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Multilanguage : MonoBehaviour
{
    public Language language = Language.English;

    public static Multilanguage instance;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
    }
}

public enum Language
{
    English,
    Japanese,
}
