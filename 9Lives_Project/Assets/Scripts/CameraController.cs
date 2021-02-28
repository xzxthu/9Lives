using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera Camera_0;
    public CinemachineVirtualCamera Camera_1;

    private void Start()
    {
        if ((!LevelManager.instance.isInMenu) && (!LevelManager.instance.isStarting))
        {
            StartCoroutine(ChangeCamera());
        }
    }

    IEnumerator ChangeCamera()
    {
        yield return new WaitForSeconds(3f);
        Camera_0.gameObject.SetActive(false);
        Camera_1.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        LevelManager.instance.isStarting = true;

        if ((!LevelManager.instance.isDrawMode) && (LevelManager.instance.isStarting) )
        {
            LevelManager.instance.Load();
            LevelManager.instance.ReDraw();
        }
    }
}
