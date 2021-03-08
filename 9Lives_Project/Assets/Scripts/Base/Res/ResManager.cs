﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// resources manager
/// </summary>
public class ResManager : BaseManager<ResManager>
{

    public T Load<T>(string name) where T : Object
    {
        T res = Resources.Load<T>(name);
        if (res is GameObject)
        {
            return GameObject.Instantiate(res);
        }

        return res;
    }


    public void LoadAsync<T>(string name, UnityAction<T> callback) where T : Object
    {
        MonoManager.GetInstance().StartCoroutine(ReallyLoadAsync(name, callback));
    }

    private IEnumerator ReallyLoadAsync<T>(string name, UnityAction<T> callback) where T : Object
    {
        ResourceRequest request = Resources.LoadAsync<T>(name);
        yield return request;
        
        if (request.asset is GameObject)
        {
            callback(GameObject.Instantiate(request.asset) as T);
            
        }
        else
        {
            Debug.Log("request.asset.name" + request.asset.name);
            callback(request.asset as T);
            
        }
    }
}