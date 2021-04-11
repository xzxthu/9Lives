using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

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

        BulletManager.GetInstance();
        MusicManager.GetInstance();
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

        if (Input.GetKeyDown(KeyCode.F))
        {
            BulletManager.GetInstance().ShootBullet(GameObject.FindWithTag("Player").transform.position + Vector3.right * 4f + Vector3.up * 0.5f,
                Vector2.left, 5f);

        }


    }
}
