using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemAnimation : MonoBehaviour
{
    public Vector2[] limits;
    public GameObject[] objects;

    public Transform player;

    private void Update()
    {
        for(int i = 0; i<limits.Length; i++)
        {
            if(limits[i][0] < player.position.y && player.position.y < limits[i][1])
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
