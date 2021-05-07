using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseOn : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private bool isShowInfo;
    private Text m_Text;
    private Color originColor;
    private int originSize;
    // Start is called before the first frame update
    void Start()
    {
        m_Text = GetComponentInChildren<Text>();
        originColor = m_Text.color;
        originSize = m_Text.fontSize;
        isShowInfo = false;
    }


    public void OnPointerExit(PointerEventData eventData)
    {
        isShowInfo = true;
        m_Text.color = originColor;
        m_Text.fontSize = originSize;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isShowInfo = false;
        m_Text.color = Color.white;
        m_Text.fontSize = originSize + 10;
    }
}
