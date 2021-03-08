using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Public MonoManager
/// 1、Method Run by Frame Here
/// 2、 Coroutine Methods is Here
/// Singleton Pattern
/// </summary>
public class MonoManager : BaseManager<MonoManager>
{
    private MonoController controller;

    public MonoManager()
    {
        GameObject obj = new GameObject("MonoController");
        obj.AddComponent<MonoController>();
        controller = obj.GetComponent<MonoController>();
    }

    /// <summary>
    /// Add Events Run by Frames/Update()
    /// </summary>
    /// <param name="fun"></param>
    public void AddUpdateListener(UnityAction fun)
    {
        controller.AddUpdateListener(fun);
    }

    /// <summary>
    /// delete Event Run by Frames
    /// </summary>
    /// <param name="fun"></param>
    public void RemoveUpdateListener(UnityAction fun)
    {
        controller.RemoveUpdateListener(fun);
    }


    /// <summary>
    /// Add Events Run by Physics/FixedUpdate()
    /// </summary>
    /// <param name="fun"></param>
    public void AddFixedUpdateListener(UnityAction fun)
    {
        controller.AddFixedUpdateListener(fun);
    }

    /// <summary>
    /// Delete  Events Run by physics time
    /// </summary>
    /// <param name="fun"></param>
    public void RemoveFixedUpdateListener(UnityAction fun)
    {
        controller.RemoveFixedUpdateListener(fun);
    }


    //Coroutine Methods (Same as MonoBehavior)
    public Coroutine StartCoroutine(IEnumerator routine)
    {
        return controller.StartCoroutine(routine);
    }
    
    public Coroutine StartCoroutine(UnityAction u1 = null,float time = 0,UnityAction u2 =null)
    {
        
        return controller.StartCoroutine(coroEnumerator(u1,time,u2));
    }
    
    public void StopAllCoroutines()
    {
        controller.StopAllCoroutines();
    }

    public void StopCoroutine(IEnumerator routine)
    {
        controller.StopCoroutine(routine);
    }

    public void StopCoroutine(Coroutine routine)
    {
        controller.StopCoroutine(routine);
    }

    public void StopCoroutine(string methodName)
    {
        controller.StopCoroutine(methodName);
    }

    IEnumerator coroEnumerator(UnityAction u1 = null,float time = 0,UnityAction u2 =null)
    {
        if (u1 != null)
            u1();
        yield return new WaitForSeconds(time);
        if (u2 != null)
            u2();
    }
}
