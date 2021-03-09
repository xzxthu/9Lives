﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActor : Actor
{
    // pamameters for move and animation control
    public float maxWalkSpeed = 0.5f;                           //最大速度 max speed of walking
    public float maxRunSpeed = 1f;                              //最大速度 max spped of running
    public float accelerate = 1f;                               //加速 acclecrate of running
    public float decelerateOnGround = 1f;                       //在地面上才横向减速（空中不减速） decelerate on ground
    public float decelerateInAir = 1f;                          //空中减速 decelerate in air
    public float changeDirectionDecelerate = 2f;                //转向会横向减速 decelerate when turn
    public float speedForHighLandding = -15;                    //大落地动作的下坠速度 velocity for play high-landing animation
    public float jumpForce;
    public float jumpHeightTime;

    // references and pamameters for feet's methods
    public Transform feetPos;
    public float feetCheckRadius;
    public Transform Left_Leg;
    public Transform Right_Leg;
    public float handCheckRadius;
    public Transform Left_BackLeg;
    public Transform Right_BackLeg;
    public LayerMask ground;

    #region private pamameters
    // private pamameters
    [HideInInspector] public float moveInput;
    [HideInInspector] public float recordMoveInput;             
    [HideInInspector] public float speed = 0f;
    [HideInInspector] public float jumpTimer;
    [HideInInspector] public float acTimer;
    [HideInInspector] public float horizontal;

    [HideInInspector] public bool isGround;
    [HideInInspector] public bool isLeftHandCatch = false;
    [HideInInspector] public bool isRightHandCatch = false;
    [HideInInspector] public bool isTurnAround = false;
    [HideInInspector] public bool isJumpClimbSuccess = false;
    [HideInInspector] public bool isHoriz = false;
    [HideInInspector] public bool isHanging = false;
    [HideInInspector] public bool isPressSpace = false;

    // references of components
    [HideInInspector] public Animator anim;
    [HideInInspector] public Transform catTransform;
    [HideInInspector] public Rigidbody2D rigid;


    // singleton
    public static PlayerActor instance;
    #endregion


    #region initialization of PlayerActor
    void Awake()
    {
        catTransform = this.transform;
        anim = GetComponent<Animator>();
        if (anim == null)
        {
            anim = GetComponentInChildren<Animator>();
        }
        rigid = GetComponent<Rigidbody2D>();
        horizontal = transform.localScale.x;  

        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }

        InitState();
        InitCurState();
    }

    protected override void InitState() // Add actor states to dictionary
    {
        _actorStateDic[ActorStateType.Idle] = new IdleState();
        _actorStateDic[ActorStateType.Run] = new RunState();
        _actorStateDic[ActorStateType.Jump] = new JumpState();
        _actorStateDic[ActorStateType.Hang] = new HangState();
    }

    protected override void InitCurState()
    {
        _curState = _actorStateDic[ActorStateType.Idle];
        _curState.Enter(this);
    }
    #endregion



    public void UpdateInActor()
    {
        moveInput = Input.GetAxisRaw("Horizontal");
        MirrorDirection();
        CalculateFeetPoint();
        isGround = Physics2D.OverlapCircle(feetPos.position, feetCheckRadius, ground);
        isLeftHandCatch = Physics2D.OverlapCircle(Left_Leg.position, handCheckRadius, ground);
        isRightHandCatch = Physics2D.OverlapCircle(Right_Leg.position, handCheckRadius, ground);
    }

    private void MirrorDirection()
    {
        if (moveInput > 0.1f)//左右镜像
        {
            transform.localScale = new Vector3(horizontal, transform.localScale.y, transform.localScale.z);
        }
        if (moveInput < -0.1f)
        {
            transform.localScale = new Vector3(-horizontal, transform.localScale.y, transform.localScale.z);
        }
    }

    private void CalculateFeetPoint()
    {
        float updateFeetX = (Left_BackLeg.position.x + Right_BackLeg.position.x) * 0.5f;
        feetPos.position = new Vector3(updateFeetX, feetPos.position.y, feetPos.position.z); //根据后脚位置更新
    }

    public void CheckAutoDown()
    {
        if (!isLeftHandCatch && !isRightHandCatch && isGround && rigid.velocity.y == 0 && !anim.GetBool("isJumping")) //平的时候手出地面范围了，无输入的时候不判定
        {
            if (moveInput != 0)
            {
                float offsetX = (PlayerActor.instance.transform.position.x - PlayerActor.instance.Left_Leg.position.x) * 1f *
            (Mathf.Abs(PlayerActor.instance.rigid.velocity.x) / PlayerActor.instance.maxRunSpeed); //往身体方向挪一点，速度越大越远
                Vector3 offset = new Vector3(offsetX, 0.5f, 0);
                //Debug.Log(offset.x);

                //offset = Vector3.zero;
                Ray2D rayUpLeft = new Ray2D(PlayerActor.instance.Left_Leg.position + offset, Vector2.down);
                Ray2D rayUpRight = new Ray2D(PlayerActor.instance.Right_Leg.position + offset, Vector2.down);

                RaycastHit2D infoLeft = Physics2D.Raycast(rayUpLeft.origin, Vector2.down, 1.5f, PlayerActor.instance.ground);
                RaycastHit2D infoRight = Physics2D.Raycast(rayUpRight.origin, Vector2.down, 1.5f, PlayerActor.instance.ground);

                //Debug.DrawRay(rayUpLeft.origin, Vector2.down, Color.yellow);
                //Debug.DrawRay(rayUpRight.origin, Vector2.down, Color.yellow);

                if (infoLeft.collider == null && infoRight.collider == null)
                {
                    Level_2_Manager.instance.isDown = true;
                }
            }
        }
    }
}