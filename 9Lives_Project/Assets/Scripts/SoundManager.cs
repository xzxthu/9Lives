using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public Transform sewerPosition;
    public Transform streetPosition;
    public Transform mallPosition;

    
    public void OpeningInTimeline()
    {
        MusicManager.GetInstance().PlaySE("1_Sewer", true, sewerPosition);
        MusicManager.GetInstance().PlaySE("2_Street", true, streetPosition);
        MusicManager.GetInstance().PlaySE("3_Mall", true, mallPosition);
    }

}
