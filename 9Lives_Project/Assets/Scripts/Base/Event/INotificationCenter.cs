using System;
using System.Collections.Generic;
using UnityEngine;

public class NotificationCenter : INotificationCenter
{
    public static INotificationCenter singleton;

    public NotificationCenter() : base()
    {
    }


    public static INotificationCenter GetInstance()
    {
        if (singleton == null)
            singleton = new NotificationCenter();
        return singleton;
    }
}

public abstract class INotificationCenter
{
    public Dictionary<string, EventHandler> eventTable;
    public Dictionary<string, EventHandler<TransferCharacterEventArgs>> transferEventTable;
    private event EventHandler tempNullHandler;
    private event EventHandler<TransferCharacterEventArgs> tempNullTransferHandler;

    public INotificationCenter()
    {
        eventTable = new Dictionary<string, EventHandler>();
        transferEventTable = new Dictionary<string, EventHandler<TransferCharacterEventArgs>>();
    }

    public void PostNotification(string name)
    {
        this.PostNotification(name, null, EventArgs.Empty);
    }

    public void PostNotification(string name, object sender)
    {
        this.PostNotification(name, sender, EventArgs.Empty);
    }

    public void PostNotification(string name, object sender, EventArgs e)
    {
        Debug.Log("Error Event Name :" + name);
        if (eventTable.ContainsKey(name))
        {
            if (eventTable[name] != null) 
            {
                eventTable[name](sender, e);
            }
        }
    }

    public void PostNotification(string name, TransferCharacterEventArgs e)
    {
        this.PostNotification(name, null, e);
    }

    public void PostNotification(string name, object sender, TransferCharacterEventArgs e)
    {
        if (transferEventTable.ContainsKey(name))
        {
            transferEventTable[name](sender, e);
        }
    }

    public void AddEventHandler(string name, EventHandler eventHandler)
    {
        //Key[name]
        if (!eventTable.ContainsKey(name))
        {
            eventTable[name] = tempNullHandler;
        }

        eventTable[name] += eventHandler;
        if (eventTable.ContainsValue(tempNullHandler))
        {
            eventTable[name] -= tempNullHandler;
        }
    }
    public void AddEventHandler(string name, EventHandler<TransferCharacterEventArgs> eventHandler) // Event with parameter
    {
        if (!transferEventTable.ContainsKey(name))
        {
            transferEventTable[name] = tempNullTransferHandler;
        }

        transferEventTable[name] += eventHandler;
        if (transferEventTable.ContainsValue(tempNullTransferHandler))
        {
            transferEventTable[name] -= tempNullTransferHandler;
        }
    }

    public void RemoveEventHandler(string name, EventHandler eventHandler)
    {
        eventTable[name] -= eventHandler;
    }

    public void RemoveEventHandler(string name, EventHandler<TransferCharacterEventArgs> eventHandler)
    {
        transferEventTable[name] -= eventHandler;
    }
}

public class TransferCharacterEventArgs : EventArgs
{
    public Vector3 transferPoint { get; set; }
}