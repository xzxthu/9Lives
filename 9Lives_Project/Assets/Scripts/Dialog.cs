using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    public Vector2 Scale = Vector2.one;
    public Transform background;
    public RectTransform text;

    public string[] dialogs;

    private Vector2 originBackgroundScale;
    private Vector2 originTextScale;

    private TypewriterEffect typer;

    private void Awake()
    {
        text.GetComponent<Text>().text = dialogs[0];
    }

    private void Start()
    {
        background.localScale = new Vector3(background.localScale.x * Scale[0], background.localScale.y * Scale[1] ,1);
        text.sizeDelta = new Vector2(text.sizeDelta[0] * Scale[0], text.sizeDelta[1] * Scale[1]);

        originBackgroundScale = background.localScale;
        originTextScale = text.sizeDelta;

        typer = GetComponentInChildren<TypewriterEffect>();
    }


}
