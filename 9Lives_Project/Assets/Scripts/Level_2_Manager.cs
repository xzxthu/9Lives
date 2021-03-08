using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_2_Manager : MonoBehaviour
{
    public bool isDown; //角色向下one way
    public int nowGround = 0; //0为没有
    public bool isHanging;


    public static Level_2_Manager instance;

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        if(Input.GetKeyDown(KeyCode.F))
        {
            BulletManager.GetInstance().ShootBullet(Vector2.zero,Vector2.up,1f);
        }
    }
}
