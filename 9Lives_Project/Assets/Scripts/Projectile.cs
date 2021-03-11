using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public ProjectileType projectileType;
    public Transform gunPort;
    public float shootingSpeed;
    public float shootingFrequency;

    [HideInInspector] public bool isSurprise;

    private float timer;
    private float rotateAngel;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rotateAngel = 0; 
        if(projectileType==ProjectileType.surprise)
        {
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    private void Update()
    {
        switch (projectileType)
        {
            case ProjectileType.normal:
                {
                    rotateAngel = 0;
                    ShootTimer();
                }
                break;
            case ProjectileType.track:
                {
                    TrackPlayer();
                    ShootTimer();
                }
                break;
            case ProjectileType.surprise:
                {
                    TrackPlayer();
                    if (isSurprise)
                    {
                        isSurprise = false;
                        gameObject.GetComponent<SpriteRenderer>().enabled = true;
                        anim.SetTrigger("QuickShootTrigger");
                    }
                }
                break;
        }
        
    }

    private void ShootTimer()
    {
        if(timer>0)
        {
            timer -= Time.deltaTime;
        }
        else
        {
            timer = 0;
            timer = shootingFrequency;
            anim.SetTrigger("ShootTrigger");
        }
    }

    private void TrackPlayer()
    {
        Vector3 v = GameObject.Find("HurtArea").transform.position - transform.position;
        v.z = 0;
        rotateAngel = Vector3.SignedAngle(Vector3.right, v, Vector3.forward);
        Debug.Log(rotateAngel);
        rotateAngel = Mathf.Clamp(rotateAngel ,- 30,60);
        transform.rotation = Quaternion.Euler(0, 0, rotateAngel);
        
    }


    public void ShootInAnimation()
    {
        Vector2 shootDir = gunPort.position.x > transform.position.x ? Vector2.right : Vector2.left;
        BulletManager.GetInstance().ShootBullet(gunPort.position, Quaternion.Euler(0f, 0f, rotateAngel) * shootDir, shootingSpeed);
    }




}



public enum ProjectileType
{
    normal,
    track,
    surprise,
}
