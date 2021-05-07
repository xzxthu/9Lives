using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour
{
    public GameObject EndingTimeline;
    public GameObject EndingCamera;
    public Animator Sky;
    public GameObject Flocks;
    public CatFace CatFace;


    public Dialog[] dialogs;
    private int nowPlayingDialog;
    private bool dialogStarted;

    private void Awake()
    {
        EndingTimeline.SetActive(false);
        nowPlayingDialog = 0;
        
    }

    private void Update()
    {
        if(dialogStarted)
        {
            if(dialogs[nowPlayingDialog].hasPlayedOneTime)
            {
                if (nowPlayingDialog == dialogs.Length - 1) return;

                nowPlayingDialog++;
                dialogs[nowPlayingDialog].AutoPlay();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
           EndingTimeline.SetActive(true);
           PlayerActor.instance.transform.localScale = new Vector3(Mathf.Abs(PlayerActor.instance.transform.localScale.x), PlayerActor.instance.transform.localScale.y, PlayerActor.instance.transform.localScale.z); ;
        }
    }

    public void StartOfEnding()
    {
        LevelManager.instance.dontMove = true;
        EndingCamera.SetActive(true);
        
        Flocks.SetActive(false);

        MusicManager.GetInstance().StopAllSeAndBgm();
        MusicManager.GetInstance().PlayBGM("FinalLevel");
    }

    public void Ending_2()
    {
        Sky.gameObject.SetActive(true);
        Sky.SetTrigger("Start");
        CatFace.SetFaceBool("isLookingUp", true);
    }

    public void Ending_3()
    {
        dialogs[0].AutoPlay();
        dialogStarted = true;
    }

    public void PlayCasting()
    {
        SceneManager.LoadScene("Credits");
    }
}
