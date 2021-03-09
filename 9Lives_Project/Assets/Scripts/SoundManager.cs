using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public Transform sewerPosition;
    public Transform streetPosition;
    public Transform mallPosition;
    public Animator anim;

    private bool isPlayingRun = false;

    public static SoundManager instance;
    private void Awake()
    {
        //DontDestroyOnLoad(this.gameObject);
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

    private void Start()
    {
        MusicManager.GetInstance().PlaySE("1_Sewer", true, sewerPosition);
        MusicManager.GetInstance().PlaySE("2_Street", true, streetPosition);
        MusicManager.GetInstance().PlaySE("3_Mall", true, mallPosition);
    }

    private void FixedUpdate()
    {
        if(anim.GetBool("isWalking") || anim.GetBool("isRunning"))
        {
            if(!isPlayingRun)
            {
                isPlayingRun = true;
                MusicManager.GetInstance().PlayBGM("Walk1", true);
            }
            
        }
        else
        {
            isPlayingRun = false;
            MusicManager.GetInstance().StopBGM("Walk1");
        }

        

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MusicManager.GetInstance().PlayBGM("Jump1");
        }

    }


}
