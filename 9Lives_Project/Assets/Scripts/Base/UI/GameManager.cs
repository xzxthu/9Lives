using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [HideInInspector] public bool isPause = false;
    [HideInInspector] public string curSceneName;            // 当前场景名
    [HideInInspector] public Camera mainCamera;              // 主摄像机

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !LevelManager.instance.TestMode)
        {
            curSceneName = SceneManager.GetActiveScene().name;

            // EventSystem不存在的话
            if (!GameObject.Find("EventSystem"))
            {
                Debug.Log(string.Format("{0}场景没有EventSystem", curSceneName));
            }

            UIManager.GetInstance()
                .ShowPanel<SettingPanel>("SettingPanel", UILayer.TOP, (res) => { res.Show(true); });
        }
    }
}