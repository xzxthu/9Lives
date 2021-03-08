using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Public Mono
/// Please use MonoManager!
/// </summary>
public class MonoController : MonoBehaviour
{
    private event UnityAction updateEvent;
    private event UnityAction fixedUpdateEvent;
    
    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    
    private void Update()
    {
        if(updateEvent != null)
        {
            updateEvent();
        }
    }
    void FixedUpdate()
    {
        if(fixedUpdateEvent != null)
        {
            fixedUpdateEvent();
        }
    }
    

    /// <summary>
    /// Add Events Run by Frames/Update()
    /// </summary>
    /// <param name="fun"></param>
    public void AddUpdateListener(UnityAction fun)
    {
        updateEvent += fun;
    }

    /// <summary>
    /// delete Event Run by Frames
    /// </summary>
    /// <param name="fun"></param>
    public void RemoveUpdateListener(UnityAction fun)
    {
        updateEvent -= fun;
    }

    /// <summary>
    /// Add Events Run by Physics/FixedUpdate()
    /// </summary>
    /// <param name="fun"></param>
    public void AddFixedUpdateListener(UnityAction fun)
    {
        fixedUpdateEvent += fun;
    }

    /// <summary>
    /// Delete  Events Run by physics time
    /// </summary>
    /// <param name="fun"></param>
    public void RemoveFixedUpdateListener(UnityAction fun)
    {
        fixedUpdateEvent -= fun;
    }
}
