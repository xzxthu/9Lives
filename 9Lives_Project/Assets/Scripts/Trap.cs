using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    public GameObject[] needToOpenProjectile;
    public GameObject[] needToHideProjectile;
    
    public float triggerDistance = 5f;

    private Transform player;
    private void Start()
    {
        player = GameObject.Find("CatController").transform;
    }

    private void Update()
    {
        if (Vector3.Distance(player.position,transform.position) < triggerDistance)
        {
            for (int i = 0; i < needToOpenProjectile.Length; i++)
            {
                needToOpenProjectile[i].SetActive(true);
                if (needToOpenProjectile[i].GetComponent<Projectile>().projectileType == ProjectileType.surprise)
                    needToOpenProjectile[i].GetComponent<Projectile>().isSurprise = true;
            }

            for (int i = 0; i < needToHideProjectile.Length; i++)
            {
                needToHideProjectile[i].SetActive(false);

            }

        }
    }
}
