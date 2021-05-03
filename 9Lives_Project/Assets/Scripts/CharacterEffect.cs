using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEffect : MonoBehaviour
{

    public Transform Accelerate;
    public Transform Jump_1;
    public Transform Jump_2;
    public Transform Decelerate;
    public Transform Catch;
    public Transform Touchdown_1;
    public Transform Touchdown_2;
    public Transform Hitted;

    private Transform worldParent;
    private Dictionary<CharacterEffectType, Transform> effectDic = new Dictionary<CharacterEffectType, Transform>();
    private Dictionary<CharacterEffectType, string> triggerDic = new Dictionary<CharacterEffectType, string>();
    private Dictionary<CharacterEffectType, Vector3> originPosDic = new Dictionary<CharacterEffectType, Vector3>();
    private Dictionary<CharacterEffectType, Vector3> originSclDic = new Dictionary<CharacterEffectType, Vector3>();
    // Start is called before the first frame update
    void Start()
    {
        worldParent = gameObject.transform.parent.transform.parent;

        effectDic[CharacterEffectType.Accelerate] = Accelerate;
        triggerDic[CharacterEffectType.Accelerate] = "Accelerate";
        originPosDic[CharacterEffectType.Accelerate] = Accelerate.localPosition;
        originSclDic[CharacterEffectType.Accelerate] = Accelerate.localScale;

        effectDic[CharacterEffectType.Jump_1] = Jump_1;
        triggerDic[CharacterEffectType.Jump_1] = "Jump";
        originPosDic[CharacterEffectType.Jump_1] = Jump_1.localPosition;
        originSclDic[CharacterEffectType.Jump_1] = Jump_1.localScale;

        effectDic[CharacterEffectType.Jump_2] = Jump_2;
        triggerDic[CharacterEffectType.Jump_2] = "Jump";
        originPosDic[CharacterEffectType.Jump_2] = Jump_2.localPosition;
        originSclDic[CharacterEffectType.Jump_2] = Jump_2.localScale;

        effectDic[CharacterEffectType.Decelerate] = Decelerate;
        triggerDic[CharacterEffectType.Decelerate] = "Decelerate";

        effectDic[CharacterEffectType.Catch] = Catch;
        triggerDic[CharacterEffectType.Catch] = "Catch";

        effectDic[CharacterEffectType.Touchdown_1] = Touchdown_1;
        triggerDic[CharacterEffectType.Touchdown_1] = "Touchdown";
        originPosDic[CharacterEffectType.Touchdown_1] = Touchdown_1.localPosition;
        originSclDic[CharacterEffectType.Touchdown_1] = Touchdown_1.localScale;

        effectDic[CharacterEffectType.Touchdown_2] = Touchdown_2;
        triggerDic[CharacterEffectType.Touchdown_2] = "Touchdown";
        originPosDic[CharacterEffectType.Touchdown_2] = Touchdown_2.localPosition;
        originSclDic[CharacterEffectType.Touchdown_2] = Touchdown_2.localScale;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void PlayCharacterEffect(CharacterEffectType type, bool dontMove = false)
    {
        if(dontMove)
            effectDic[type].parent = worldParent;

        effectDic[type].GetComponent<Animator>().SetTrigger(triggerDic[type]); 
    }

    public void PlayHittedEffect(float angle, Vector3 pos)
    {
        Hitted.position = pos;
        Hitted.eulerAngles = new Vector3(0, 0, angle);
        Hitted.GetComponent<Animator>().SetTrigger("Hitted");
    }

    public void ResetPosition(CharacterEffectType type)
    {
        effectDic[type].parent = transform;
        effectDic[type].localPosition = originPosDic[type];
        effectDic[type].localScale = originSclDic[type];
    }

}

public enum CharacterEffectType
{
    Accelerate,
    Jump_1,
    Jump_2,
    Decelerate,
    Catch,
    Touchdown_1,
    Touchdown_2,
}
