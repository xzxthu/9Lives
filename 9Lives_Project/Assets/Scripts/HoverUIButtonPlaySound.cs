using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverUIButtonPlaySound :  MonoBehaviour, IPointerEnterHandler
{

    public AudioClip Sound;

    private Text Text;
    private Color origin;

    private void Start()
    {
        Text = GetComponentInChildren<Text>();
        origin = Text.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {

        Button button = GetComponent<Button>();

        if (button == null)
        {
            // 结束
            return;
        }

        if (button.interactable)
        {
            // 音频源的引用
            AudioSource audioSource = GetComponent<AudioSource>();
            // 没有查找到音频源的情况下
            if (audioSource == null)
            {
                // 创建音频源
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            // 音频源没有正在播放的情况下
            if (!audioSource.isPlaying)
            {
                // 设置音频源属性
                // 2D音效
                audioSource.spatialBlend = 0;
                // 唤醒播放
                audioSource.playOnAwake = false;
                // 音频剪辑
                audioSource.clip = Sound;
                // 播放音频
                audioSource.Play();

            }
        }
    }

    private void ChangeTextColor()
    {

    }
}
