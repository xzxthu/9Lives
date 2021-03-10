using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Animations;

public class FootIK : MonoBehaviour
{
    public bool enableFootIK;

    public Transform Left_Leg;
    public Transform Right_Leg;

    private Vector2 checkPoint;
    private float roadHight;
    private Animator anim;

    private void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        if(PlayerActor.instance.needIK)
        {
            FootPositionSolver();
        }
        else
        {
            anim.SetFloat("Blend", 0.5f);
        }
    }

    public void FootPositionSolver()
    {
        checkPoint = new Vector2( ((Left_Leg.position + Right_Leg.position)/2).x, transform.position.y + 0.4f); // 前脚中间往上0.4

        Ray2D ray = new Ray2D(checkPoint , Vector2.down);

        Debug.DrawRay(ray.origin, ray.direction, Color.yellow);

        RaycastHit2D info = Physics2D.Raycast(ray.origin, ray.direction);

        if (info.collider != null)
        {
            if (info.collider.CompareTag("Ground"))
            {
                //Debug.Log(info.point.y);
                roadHight = Mathf.Clamp01((info.point.y - transform.position.y + 0.22f)/0.44f);
                anim.SetFloat("Blend", roadHight);
            }
        }
    }


}
