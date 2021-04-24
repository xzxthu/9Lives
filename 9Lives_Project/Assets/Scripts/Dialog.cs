using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    public bool FromDownToUp = false;
    //public bool FromRightToLeft = false;
    public bool AutoPlayDialog = false;

    public Vector2 Scale = Vector2.one;
    public Transform background;
    public RectTransform text;

    public string[] dialogs_EN;
    public string[] dialogs_JP;
    [HideInInspector] public int nowDiaNum = 0;

    [Range(1,10)]public float autoPlaySpeed = 1;

    private Vector2 originBackgroundScale;
    private Vector2 originTextScale;

    public TypewriterEffect typer;
    private bool isClosed = false;
    private bool readyAutoPlay = false;
    [HideInInspector] public bool loopPlay;

    private void Awake()
    {
        if(FromDownToUp)
            background.parent.GetComponent<Animator>().SetBool("FromDownToUp",true);
        
        
    }

    private void Start()
    {
        readyAutoPlay = false;
        ChooseLanguage(0);
        background.localScale = new Vector3(background.localScale.x * Scale[0], background.localScale.y * Scale[1] ,1);
        text.sizeDelta = new Vector2(text.sizeDelta[0] * Scale[0], text.sizeDelta[1] * Scale[1]);
        if (!FromDownToUp)
        {
            background.localPosition += new Vector3(1f * (Scale[0] - 1), -1f * (Scale[1] - 1));
            text.localPosition += new Vector3(300f * (Scale[0] - 1), -300f * (Scale[1] - 1));
        }
        else
        {
            background.localPosition += new Vector3(1f * (Scale[0] - 1), 1f * (Scale[1] - 1));
            text.localPosition += new Vector3(300f * (Scale[0] - 1), 300f * (Scale[1] - 1));
        }

        originBackgroundScale = background.localScale;
        originTextScale = text.sizeDelta;

        //typer = GetComponentInChildren<TypewriterEffect>();
        

        background.gameObject.SetActive(false);
        //text.gameObject.SetActive(false);
        isClosed = true;

        if (AutoPlayDialog)
        {
            readyAutoPlay = true;
            loopPlay = true;
        }
    }

    private void Update()
    {
        
        if(readyAutoPlay)
        {
            readyAutoPlay = false;
            GoNextText();
            float time = GetWordsNumber() * 0.1f / autoPlaySpeed + 2f;
            StartCoroutine(PlayLines(time));
            
        }

    }

    public void GoNextText()
    {

        if(isClosed)
        {
            nowDiaNum = 0;
            background.gameObject.SetActive(true);
            background.parent.GetComponent<Animator>().SetTrigger("In");
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
            background.parent.GetComponent<Animator>().SetTrigger("In");
        }
        else
        {
            nowDiaNum = 0;
            background.parent.GetComponent<Animator>().SetTrigger("Out");
            //background.gameObject.SetActive(false);
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

    private int GetWordsNumber()
    {
        switch (Multilanguage.instance.language)
        {
            case Language.English:
                return dialogs_EN[nowDiaNum].Length;
                break;
            case Language.Japanese:
                return dialogs_JP[nowDiaNum].Length;;
                break;
            default:
                return 0;
        }
    }

    public void AutoPlay(bool Loop = false)
    {
        readyAutoPlay = true;
        loopPlay = Loop;


    }

    private IEnumerator PlayLines(float time)
    {
        //Debug.Log("Play nowDiaNum " + nowDiaNum);
        yield return new WaitForSeconds(time);
        if (!isClosed || loopPlay)
        {
            readyAutoPlay = true;
        }
        
    }
}
