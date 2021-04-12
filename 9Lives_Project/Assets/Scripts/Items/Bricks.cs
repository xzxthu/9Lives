using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bricks : MonoBehaviour
{
    public float timeIntervel;
    private float timeToShoot;
    private float timer;
    private bool needToShoot;

    private void Start()
    {
        timeToShoot = timeIntervel;
    }
    private void Update()
    {
        if (!needToShoot) return;

        if(timer < timeToShoot)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
            timeToShoot = ((float)Random.Range(5, 15) / 10f) * timeIntervel;
            Vector2 gunPort = new Vector2(Random.Range(-7,7),transform.position.y);
            BulletManager.GetInstance().ShootBullet(gunPort, Vector3.down, 20f, BulletType.brick);
        }

    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            needToShoot = true;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            needToShoot = false;
        }
    }
}
