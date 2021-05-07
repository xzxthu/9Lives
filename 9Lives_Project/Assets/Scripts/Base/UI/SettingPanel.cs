using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingPanel : BasePanel
{
    private GameObject SettingAudio; // 设置音量

    private bool isShowCursor = false; // 是否需要显示鼠标
    

    private void Start()
    {
        transform.parent.transform.parent.GetComponent<Canvas>().sortingOrder = 1;

        SettingAudio = transform.Find("Bg/SettingAudio").gameObject;
        GetControl<Button>("BtnAudio").onClick.AddListener((() =>
        {
            SettingAudio.SetActive(true);
        }));
        
        GetControl<Slider>("SliderBGM").onValueChanged.AddListener((newValue =>
        {
            GetControl<Text>("TexPercentageBGM").text = string.Format("{0:0}%", newValue * 100.0);
            MusicManager.GetInstance().SetAllSEValue(newValue);
        }));

        GetControl<Slider>("SliderSE").onValueChanged.AddListener((newValue =>
        {
            GetControl<Text>("TexPercentageSE").text = string.Format("{0:0}%", newValue * 100.0);
            MusicManager.GetInstance().SetAllBGMValue(newValue);
        }));

        GetControl<Button>("BtnBack").onClick.AddListener(() => { Hide(); });
        GetControl<Button>("BtnBackToMainMenu").onClick.AddListener((() =>
        {
            Hide();
            MusicManager.GetInstance().StopAllSeAndBgm();
            SceneManager.LoadScene("StartMenu");

            
        }));
    }

    public void Show(bool afterHideShowCursor = false)
    {

        isShowCursor = afterHideShowCursor;
        Time.timeScale = 0f;
        MusicManager.GetInstance().PauseAllSeAndBgm();
        //Cursor.lockState = CursorLockMode.Confined;
        //Cursor.visible = true;

        System.GC.Collect();
        GameManager.Instance.isPause = true;
    }

    public override void Hide()
    {
        if (!isShowCursor)
        {
            //Cursor.lockState = CursorLockMode.Confined;
            //Cursor.visible = false;
        }

        // 使下次打开时，默认是音量设置界面
        //SettingAudio.SetActive(true);

        Time.timeScale = 1.0f;
        MusicManager.GetInstance().UnPauseAllSeAndBgm();
        GameManager.Instance.isPause = false;
        gameObject.SetActive(false);

    }


}