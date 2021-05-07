using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// layers in UI
/// </summary>
public enum UILayer
{
    BOT,
    MID,
    TOP,
    SYSTEM
}


/// <summary>
/// UIManager
/// controll all panel
/// </summary>
public class UIManager : BaseManager<UIManager>
{
    public Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();


    private Transform bot;
    private Transform mid;
    private Transform top;
    private Transform system;

    public UIManager()
    {
        GameObject obj = ResManager.GetInstance().Load<GameObject>("Prefabs/UI/Canvas");
        Transform canvas = obj.transform;
        GameObject.DontDestroyOnLoad(obj);//**

        bot = canvas.Find("Bot");
        mid = canvas.Find("Mid");
        top = canvas.Find("Top");
        system = canvas.Find("System");

        //obj = ResManager.GetInstance().Load<GameObject>("Prefabs/UI/EventSystem");
        //GameObject.DontDestroyOnLoad(obj);
    }


    public void ShowPanel<T>(string panelName, UILayer layer = UILayer.MID, UnityAction<T> callback = null)
        where T : BasePanel
    {
        if (panelDic.ContainsKey(panelName))
        {
            panelDic[panelName].gameObject.SetActive(true);
            panelDic[panelName].Show();
            if (callback != null)
            {
                callback(panelDic[panelName] as T);
            }
            return;
        }

        ResManager.GetInstance().LoadAsync<GameObject>("Prefabs/UI/" + panelName, (obj) =>
        {
            Transform parent = bot;
            switch (layer)
            {
                case UILayer.MID:
                    parent = mid;
                    break;
                case UILayer.TOP:
                    parent = top;
                    break;
                case UILayer.SYSTEM:
                    parent = system;
                    break;
            }

            //Initialization
            obj.transform.SetParent(parent);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            obj.name = panelName;
            (obj.transform as RectTransform).offsetMax = Vector2.zero;
            (obj.transform as RectTransform).offsetMin = Vector2.zero;

            T panel = obj.GetComponent<T>();
            if (callback != null)
            {
                callback(panel);
            }

            panel?.Show();
            if (!panelDic.ContainsKey(panelName))
            {
                panelDic.Add(panelName, panel);
            }
        });
    }

    /// <summary>
    /// hide UI panel
    /// </summary>
    /// <param name="panelName"></param>
    public void HidePanel(string panelName)
    {
        if (panelDic.ContainsKey(panelName))
        {
            panelDic[panelName].Hide();
            GameObject.Destroy(panelDic[panelName].gameObject);
            panelDic.Remove(panelName);
        }
    }

    /// <summary>
    /// 隐藏所有的Panel面板
    /// </summary>
    public void HideAllWordPanel()
    {
        //WordManager.Instance.StopWordManagerAllCoroutine();
    }
    

    /// <summary>
    /// hide Word Panel
    /// </summary>
    /// <param name="panelName"></param>
    public void HideWordPanel(string panelName)
    {
        if (panelDic.ContainsKey(panelName))
        {
            panelDic[panelName].Hide();
        }
    }

    /// <summary>
    /// clear all in dictionary
    /// </summary>
    public void Clear()
    {
        var keys = panelDic.Keys;
        List<string> k = new List<string>();
        foreach (var panelName in keys)
        {
            k.Add(panelName);
        }

        foreach (var panelName in k)
        {
            HidePanel(panelName);
        }
    }
}