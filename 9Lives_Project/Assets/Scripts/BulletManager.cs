using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class BulletManager : BaseManager<BulletManager>
{
    private List<Bullet> bulletList = new List<Bullet>();

    public BulletManager()
    {
        MonoManager.GetInstance().AddUpdateListener(Update);
        for (int i = 0; i < 21; ++i) //20 bullets in a poll
        {
            PoolManager.GetInstance().GetObject("Bullet/Bullet_1",
                (res => { PoolManager.GetInstance().PushObject("Bullet/Bullet_1", res); }));

        }
    }


    public void Update()
    {
        for (int i = bulletList.Count - 1; i >= 0; --i)
        {
            if (bulletList[i].isOutOfScreen)
            {
                Debug.Log("检测了超出屏幕！");
                PoolManager.GetInstance().PushObject("Bullet/Bullet_1", bulletList[i].gameObject);
                bulletList.RemoveAt(i);
            }
        }

        for (int i = bulletList.Count - 1; i >= 0; --i)
        {
            if (bulletList[i] == null)
                bulletList.RemoveAt(i);
        }

    }

    public void ShootBullet(Vector2 startPoint, Vector2 direction, float speed)
    {
        PoolManager.GetInstance().GetObject("Bullet/Bullet_1", (bullet) =>
        {
            Bullet blt =  bullet.GetComponent<Bullet>();
            blt.startPoint = startPoint;
            blt.direction = direction.normalized;
            blt.speed = speed;
        });
    }
}
