using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    public GameObject QuitEffect;

    private bool hasPass;
    

    private void Start()
    {
        hasPass = false;
        MusicManager.GetInstance().PlayBGM("Credits");
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!hasPass)
            {
                hasPass = true;
                StartCoroutine(ReturnStartMenu());

            }
        }
    }

    public void TheEnd()
    {
        MusicManager.GetInstance().StopAllSeAndBgm();
        SceneManager.LoadScene("StartMenu");
    }

    private IEnumerator ReturnStartMenu()
    {
        QuitEffect.SetActive(true);
        yield return new WaitForSeconds(2f);
        MusicManager.GetInstance().StopAllSeAndBgm();
        SceneManager.LoadScene("StartMenu");

    }
}
