using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        MusicManager.GetInstance().PlayBGM("GameplayTheme");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
