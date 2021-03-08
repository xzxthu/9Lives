using System;
using System.Collections.Generic;
using UnityEngine;

public class Observer : MonoBehaviour
{
    private INotificationCenter center;
    private Dictionary<string, EventHandler> handlers;
    private Dictionary<string, EventHandler<TransferCharacterEventArgs>> transferHandlers;

    private void Awake()
    {
        handlers = new Dictionary<string, EventHandler>();
        transferHandlers = new Dictionary<string, EventHandler<TransferCharacterEventArgs>>();
        center = NotificationCenter.GetInstance();
    }

    public void AddEventHandler(string name, EventHandler handler)
    {
        center.AddEventHandler(name, handler);
        handlers.Add(name, handler);
    }

    private void OnDestroy()
    {
        foreach (KeyValuePair<string, EventHandler> kvP in handlers)
        {
            center.RemoveEventHandler(kvP.Key, kvP.Value);
        }
    }

    private void Start()
    {
    }
}