using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base Class of Singleton Pattern
/// </summary>
public class BaseManager<T> where T:new()
{
    private static T instance;

    /// <summary>
    /// Get Instance
    /// </summary>
    public static T GetInstance()
    {
        if(instance == null)
        {
            instance = new T();
        }
        return instance;
    }
}
