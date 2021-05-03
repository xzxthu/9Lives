using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crane : MonoBehaviour
{
    public Transform platform;

    private Vector3 endPos;
    private float originLength;
    private Vector3 originPos;
    private Vector3 originPlatformPos;
    private float newScaleY;
    // Start is called before the first frame update
    void Start()
    {
        originLength = Vector3.Distance(platform.position, transform.position) * 2f;
        endPos = platform.position + Vector3.up * originLength;
        originPos = transform.position;
        originPlatformPos = platform.position;
    }

    // Update is called once per frame
    void Update()
    {
        newScaleY = Vector3.Distance(platform.position, endPos)/originLength;
        transform.localScale = new Vector3(transform.localScale.x, newScaleY, transform.localScale.z);
        transform.position = originPos + Vector3.up * (platform.position.y - originPlatformPos.y)/2f;
    }
}
