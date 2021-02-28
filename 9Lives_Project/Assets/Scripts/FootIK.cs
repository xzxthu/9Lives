using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Animations;

public class FootIK : MonoBehaviour
{
    public bool enableFootIK;

    public Transform Left_Leg;
    public Transform Right_Leg;
    public Transform Left_BackLeg;
    public Transform Right_BackLeg;

    public Transform Left_Leg_Target;
    public Transform Right_Leg_Target;
    public Transform Left_BackLeg_Target;
    public Transform Right_BackLeg_Target;
    public Transform Left_Leg_Mid_Target;
    public Transform Right_Leg_Mid_Target;
    public Transform Left_BackLeg_Mid_Target;
    public Transform Right_BackLeg_Mid_Target;

    public float feetCheckRadius;
    public Vector2 raycastDistance;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float Yoffset = 0f;
    public bool showSolverDebug = true;

    private float left_leg_MoveY, right_leg_MoveY;


    private void Start()
    {
    }


    private void Update()
    {
        if (!enableFootIK) return;

        CheckFootGround(Left_Leg, Left_Leg_Target, ref left_leg_MoveY);
        CheckFootGround(Right_Leg, Right_Leg_Target, ref right_leg_MoveY);

        MoveMidHeight();


    }


    private void CheckFootGround(Transform foot,Transform foot_Target, ref float footMoveY)
    {

        if (Physics2D.OverlapCircle(foot.position, feetCheckRadius, groundLayer)) //脚沾地了
        {
            FootPositionSolver(foot,foot_Target, ref footMoveY); //射线检测
        }
    }


    private void MoveMidHeight()
    {
        if (left_leg_MoveY == 0 && right_leg_MoveY == 0)
        {
            return;
        }

        Left_Leg_Mid_Target.position = new Vector3(Left_Leg_Mid_Target.position.x, Left_Leg_Mid_Target.position.y + left_leg_MoveY, Left_Leg_Mid_Target.position.z);
        Right_Leg_Mid_Target.position = new Vector3(Right_Leg_Mid_Target.position.x, Right_Leg_Mid_Target.position.y + left_leg_MoveY, Right_Leg_Mid_Target.position.z);

    }

    private void FootPositionSolver(Transform foot, Transform foot_Target, ref float footMoveY)
    {
        //射线检测
        RaycastHit footOutHit;

        if (showSolverDebug)
        {
            Debug.DrawLine(foot.position + Vector3.up * (raycastDistance.x), foot.position + Vector3.down * (raycastDistance.y), Color.yellow);
        }

        if (Physics.Raycast(foot.position + Vector3.up * (raycastDistance.x), Vector3.down * (raycastDistance.y), out footOutHit,  groundLayer))
        {
            footMoveY = footOutHit.point.y - foot.position.y;
            foot_Target.position = new Vector3(foot_Target.position.x, foot_Target.position.y + footMoveY, foot_Target.position.z);

            return;
        }
        footMoveY = 0;
    }


}
