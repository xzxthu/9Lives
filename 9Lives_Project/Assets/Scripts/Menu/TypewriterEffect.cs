using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TypewriterEffect : MonoBehaviour
{
    public bool autoPlay = false;
    public float charsPerSecond = 0.2f;//打字时间间隔
    private string words;//保存需要显示的文字

    [HideInInspector] public bool isActive = false;
    private float timer;//计时器
    private Text myText;
    [SerializeField]private int currentPos = 0;//当前打字位置

    // Use this for initialization
    void Start()
    {
        timer = 0;
        isActive = false;
        charsPerSecond = Mathf.Max(0.001f, charsPerSecond);
        myText = GetComponent<Text>();
        words = myText.text;
        myText.text = "";//获取Text的文本信息，保存到words中，然后动态更新文本显示内容，实现打字机的效果
        if(!autoPlay)
        {
            gameObject.SetActive(false);//把Dialog的start中的关闭文本挪到此处

        }
        else
        {
            StartEffect();
        }

    }

    // Update is called once per frame
    void Update()
    {
        OnStartWriter();
        //Debug.Log (isActive);
    }

    public void StartEffect()
    {
        isActive = true;
    }
    /// <summary>
    /// 执行打字任务
    /// </summary>
    void OnStartWriter()
    {

        if (isActive)
        {
            timer += Time.deltaTime;
            if (timer >= charsPerSecond)
            {//判断计时器时间是否到达
                timer = 0;
                currentPos++;
                myText.text = words.Substring(0, currentPos);//刷新文本显示内容

                if (currentPos >= words.Length-1 || currentPos >= 20)
                {
                    OnFinish();
                }
            }

        }
    }
    /// <summary>
    /// 结束打字，初始化数据
    /// </summary>
    void OnFinish()
    {
        isActive = false;
        timer = 0;
        currentPos = 0;
        myText.text = words;
    }

    public void Reset()
    {
        words = myText.text;
        myText.text = "";
        timer = 0;
        currentPos = 0;
    }


}