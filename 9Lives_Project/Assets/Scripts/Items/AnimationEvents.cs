using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public void Drop_PauseAnimation()
    {
        gameObject.GetComponentInParent<Drops>().PauseAnimation();
    }

    public void Argue_Shoot()
    {
        Vector3 gunPort = GetComponentInChildren<Transform>().position;
        BulletManager.GetInstance().ShootBullet(gunPort, Vector3.right, 12f, BulletType.pizza);
    }

    public void DialogParent_CloseBackground()
    {
        GetComponentInChildren<Transform>().gameObject.SetActive(false);
    }
}
