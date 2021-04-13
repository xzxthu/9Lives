using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAnimation : MonoBehaviour
{
    
    public Vector2[] limits;
    public GameObject[] objects;

    private Transform player;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }
    private void Update()
    {
        if (LevelManager.instance.TestMode) return;

        for(int i = 0; i<limits.Length; i++)
        {
            if(objects[i].transform.position.y-7 + limits[i].x < player.position.y && 
                player.position.y < objects[i].transform.position.y + 7 + limits[i].y)
            {
                objects[i].SetActive(true);
            }
            else
            {
                objects[i].SetActive(false);
            }
        }
    }
}
