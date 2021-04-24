using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextMultilingual : MonoBehaviour
{
    public string english;
    public string japanese;
    private Text text;

    private void Start()
    {
        text = GetComponent<Text>();
    }

    private void Update()
    {

            try
            {
                switch (Multilanguage.instance.language)
                {

                    case Language.English:
                        {
                            text.text = english;
                        }
                        break;
                    case Language.Japanese:
                        {
                            text.text = japanese;
                        }
                        break;

                    default:
                        {

                        }
                        break;
                }
            }
            catch (System.Exception e)
            {

            }
        //}
        
    }
}
