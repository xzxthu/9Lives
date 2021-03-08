using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class BasePanel : MonoBehaviour
{
    private Dictionary<string, List<UIBehaviour>> dicControl = new Dictionary<string, List<UIBehaviour>>();

    /// <summary>
    /// get all components in panel
    /// </summary>
    void Awake()
    {
        FindChildren<Button>();
        FindChildren<Text>();
        FindChildren<Slider>();
        FindChildren<Image>();
        FindChildren<Toggle>();
        FindChildren<ScrollRect>();
        FindChildren<Scrollbar>();
    }

    /// <summary>
    /// get the name
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="controlName"></param>
    /// <returns></returns>
    protected T GetControl<T>(string controlName) where T : UIBehaviour
    {
        if (dicControl.ContainsKey(controlName))
        {
            for (int i = 0; i < dicControl[controlName].Count; ++i)
            {
                if (dicControl[controlName][i] is T)
                {
                    return dicControl[controlName][i] as T;
                }
            }
        }

        return null;
    }

    /// <summary>
    /// show it
    /// </summary>
    public virtual void Show()
    {
    }

    /// <summary>
    /// hide it
    /// </summary>
    public virtual void Hide()
    {
    }

    /// <summary>
    /// find all the children components
    /// </summary>
    private void FindChildren<T>() where T : UIBehaviour
    {
        T[] btns = this.GetComponentsInChildren<T>();
        for (int i = 0; i < btns.Length; ++i)
        {
            string name = btns[i].gameObject.name;
            if (dicControl.ContainsKey(name))
            {
                dicControl[name].Add(btns[i]); // [name(Object), UIBehaviour(Component)]
            }
            else
            {
                dicControl.Add(name, new List<UIBehaviour>() {btns[i]});
            }
        }
    }
}