using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    public Vector2 Scale = Vector2.one;
    public Transform background;
    public RectTransform text;

    public string[] dialogs_EN;
    public string[] dialogs_JP;
    [HideInInspector] public int nowDiaNum = 0;

    private Vector2 originBackgroundScale;
    private Vector2 originTextScale;

    private TypewriterEffect typer;
    private bool isClosed = false;

    private void Awake()
    {
        ChooseLanguage(0);
    }

    private void Start()
    {
        background.localScale = new Vector3(background.localScale.x * Scale[0], background.localScale.y * Scale[1] ,1);
        text.sizeDelta = new Vector2(text.sizeDelta[0] * Scale[0], text.sizeDelta[1] * Scale[1]);

        originBackgroundScale = background.localScale;
        originTextScale = text.sizeDelta;

        typer = GetComponentInChildren<TypewriterEffect>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.G))
        {
            GoNextText();
        }
    }

    public void GoNextText()
    {

        if(isClosed)
        {
            nowDiaNum = 0;
            background.gameObject.SetActive(true);
            text.gameObject.SetActive(true);
            ChooseLanguage(nowDiaNum);
            typer.Reset();
            typer.isActive = true;
            isClosed = false;
            return;
        }

        if(nowDiaNum<dialogs_EN.Length-1)
        {
            nowDiaNum++;
            ChooseLanguage(nowDiaNum);
            typer.Reset();
            typer.isActive = true;
        }
        else
        {
            nowDiaNum = 0;
            background.gameObject.SetActive(false);
            text.gameObject.SetActive(false);
            isClosed = true;
        }
    }

    private void ChooseLanguage(int num)
    {
        switch(Multilanguage.instance.language)
        {
            case Language.English:
                text.GetComponent<Text>().text = dialogs_EN[num];
                break;
            case Language.Japanese:
                text.GetComponent<Text>().text = dialogs_JP[num];
                break;
        }
    }
}
