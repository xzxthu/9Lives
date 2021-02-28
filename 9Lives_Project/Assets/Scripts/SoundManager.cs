using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [HideInInspector]public  AudioSource audioSrc;
    public  AudioClip walking;
    public  AudioClip running;
    public  AudioClip sewer;
    public  AudioClip jump;

    private bool isWalking;
    private bool isRunning;
    private bool isJumping;
    private bool isPlaying;
    private bool needToStop;

    public Animator anim;

    public static SoundManager instance;
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

    private void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        audioSrc.PlayOneShot(sewer);

    }

    private void FixedUpdate()
    {

        if(anim.GetBool("isWalking"))
        {
            isWalking = true;

        }
        if(anim.GetBool("isRunning"))
        {
            isRunning = true;
        }
        
        if(isJumping)
        {
            if(!audioSrc.isPlaying)
            {
                isJumping = false;
            }
        }

        if (isPlaying)
        {
            if (needToStop)
            {
                if(!isJumping)
                {
                    audioSrc.Stop();
                    needToStop = false;
                    isPlaying = false;
                }
                
                
            }
        }
        else
        {
            if (isRunning)
            {
                audioSrc.clip = running;
                audioSrc.Play();
                audioSrc.loop = true;
                isPlaying = true;
            }
            if (isWalking)
            {
                audioSrc.clip = walking;
                audioSrc.Play();
                audioSrc.loop = true;
                isPlaying = true;
            }
            
            
        }
    }

    public void PlayRunSound()
    {
        needToStop = true;
        isRunning = true;
    }

    public void PlayJumpSound()
    {
        isJumping = true;
        audioSrc.Stop();
        audioSrc.PlayOneShot(jump);
        //Invoke("ResetJump", 1.2f);
    }

    public void StopSound()
    {

        needToStop = true; 

        isRunning = false;
        isWalking = false;
        isJumping = false;
        
    }

    private void ResetJump()
    {
        isJumping = false;
    }

}
