using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Object Pool
/// </summary>
public class PoolData
{
    //unber where the pools put 
    public GameObject parent;

    public List<GameObject> poolList;

    public PoolData(GameObject obj,GameObject pool)
    {
        parent = new GameObject(obj.name);
        parent.transform.SetParent(pool.transform);
        poolList = new List<GameObject>() { obj };
        PushObject(obj);
    }

    /// <summary>
    /// put back into pool
    /// </summary>
    public void PushObject(GameObject obj)
    {
        // Debug.Log(obj.name);
        poolList.Add(obj);
        obj.transform.SetParent(parent.transform);
        obj.SetActive(false);
    }

    /// <summary>
    /// get from pool
    /// </summary>
    public GameObject GetObject()
    {
        GameObject obj = null;
        obj = poolList[0];
        poolList.RemoveAt(0);
        obj.SetActive(true);
        return obj;
    }
}


/// <summary>
/// pool manager
/// singleto pattern
/// </summary>
public class PoolManager : BaseManager<PoolManager>
{
    public Dictionary<string, PoolData> dicPool = new Dictionary<string, PoolData>();
    public GameObject poolObj;
    /// <summary>
    /// get object
    /// </summary>
    public void GetObject(string name,UnityAction<GameObject> callback)
    {
        if(dicPool.ContainsKey(name) && dicPool[name].poolList.Count > 0)
        {
            callback(dicPool[name].GetObject());
        }
        else
        {
            ResManager.GetInstance().LoadAsync<GameObject>("Prefabs/" + name, (o) =>
            {
                
                o.name = name;
                callback(o);
            });
        }

    }

    /// <summary>
    /// put back into pool
    /// </summary>
    public void PushObject(string name, GameObject obj)    
    {
        if (poolObj == null)
        {
            // poolObj = new GameObject("Pool");
            poolObj = ResManager.GetInstance().Load<GameObject>("Prefabs/Pool/Pool");
        }

        if (dicPool.ContainsKey(name))
        {
            dicPool[name].PushObject(obj);
        }
        else
        {
            dicPool.Add(name, new PoolData(obj, poolObj));
        }
    }

    /// <summary>
    /// clear pool
    /// </summary>
    public void Clear()
    {
        dicPool.Clear();
        poolObj = null;
    }
}
