﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : ActorState
{
    private Actor _actor;

    private bool hasPlayedEffect;
    public override ActorStateType StateType
    {
        get
        {
            return ActorStateType.Jump;
        }
    }

    public override void Enter(params object[] param)
    {
        Debug.Log("JumpState Enter");
        _actor = param[0] as Actor;
        if (_actor != null)
        {
            PlayerActor.instance.anim.SetBool("isWalking", false);
            PlayerActor.instance.anim.SetBool("isRunning", false);
        }

        
        hasPlayedEffect = false;
    }

    public override void FixedUpdate()
    {
        CalculateSpeed();
        PlayerActor.instance.UpdateInActor();
        PlayAnimation();


        if (PlayerActor.instance.isHurted)
        {
            PlayerActor.instance.TransState(ActorStateType.Hurted);
            return;
        }

        if (PlayerActor.instance.isSlipping)
        {
            PlayerActor.instance.TransState(ActorStateType.Slip);
            return;
        }

        if (PlayerActor.instance.isGround && Mathf.Approximately(PlayerActor.instance.rigid.velocity.y, 0) && 
            !Mathf.Approximately(PlayerActor.instance.moveInput, 0))
        {
            PlayerActor.instance.catEffect.PlayCharacterEffect(CharacterEffectType.Touchdown_1, true);
            PlayerActor.instance.TransState(ActorStateType.Run);
            PlayerActor.instance.jumpToRun = true;
            return;
        }

        if (PlayerActor.instance.isGround && Mathf.Approximately( PlayerActor.instance.rigid.velocity.y,0))
        {
            if(Mathf.Approximately(PlayerActor.instance.rigid.velocity.x, 0))
                PlayerActor.instance.catEffect.PlayCharacterEffect(CharacterEffectType.Touchdown_2, true);
            PlayerActor.instance.catEffect.PlayCharacterEffect(CharacterEffectType.Touchdown_1, true);
            PlayerActor.instance.TransState(ActorStateType.Idle);
            MusicManager.GetInstance().PlayBGM("CatLand");
            return;
        }

        if (CheckAutoLand())
        {
            PlayerActor.instance.isHanging = true;
            PlayerActor.instance.TransState(ActorStateType.Hang);
            return;
        }
    }
    public override void Update()
    {
        CheckLongTimeJump();
        
    }


    public override void Exit()
    {
        _actor = null;
        PlayerActor.instance.skin.ChangeNormalFace();
        //Debug.Log("JumpState Exit");
        PlayerActor.instance.catFace.SetFaceBool("isCribing", false) ;
        PlayerActor.instance.catFace.SetFaceBool("isRunning", false);

    }

    private void CalculateSpeed()
    {

        if (PlayerActor.instance.moveInput * PlayerActor.instance.recordMoveInput < -0.1f) //turning
        {
            Debug.Log("空中转身");
            PlayerActor.instance.isTurnAround = true;
            PlayerActor.instance.rigid.velocity = new Vector2(-PlayerActor.instance.rigid.velocity.x/2, PlayerActor.instance.rigid.velocity.y);
        }
        else
        {
            PlayerActor.instance.isTurnAround = false;
        }

        PlayerActor.instance.recordMoveInput = PlayerActor.instance.moveInput; //record for calculate turning

        if (!Mathf.Approximately(PlayerActor.instance.moveInput, 0))//Accelerate
        {
            if (Input.GetAxis("Vertical") > 0.5f) //press "up" in air
            {
                PlayerActor.instance.speed *= 0.5f;
            }
            else
            {
                PlayerActor.instance.speed = PlayerActor.instance.maxRunSpeed;
            }
            PlayerActor.instance.rigid.velocity =
                new Vector2(PlayerActor.instance.recordMoveInput * PlayerActor.instance.speed,
                PlayerActor.instance.rigid.velocity.y);
        }
        else//Decelerate
        {
            

            if (PlayerActor.instance.speed > 0)
            {
                if (Input.GetAxis("Vertical") > 0.5f) //Decelerate
                {
                    PlayerActor.instance.speed -= Time.fixedDeltaTime * PlayerActor.instance.decelerateInAir * 2;
                }
                else
                {
                    PlayerActor.instance.speed -= Time.fixedDeltaTime * PlayerActor.instance.decelerateInAir;
                }
            }
            else
            {
                PlayerActor.instance.speed = 0;
                PlayerActor.instance.recordMoveInput = 0;
            }

            float direction = PlayerActor.instance.rigid.velocity.x > 0.1f ? 1f:-1f;

            PlayerActor.instance.rigid.velocity =
                new Vector2(((PlayerActor.instance.moveInput==0)? direction: PlayerActor.instance.moveInput)
                *  PlayerActor.instance.speed,
                PlayerActor.instance.rigid.velocity.y); //不用判断会变成0,无输入时用rigid运动方向
        }
    }

    private void PlayAnimation()
    {
        if (!PlayerActor.instance.anim.GetBool("isJumping") && !PlayerActor.instance.anim.GetBool("isJumpingHoriz"))//没有任何输入的情况下的向下动画
        {
            PlayerActor.instance.anim.SetBool("isDowning", true);

            PlayerActor.instance.catFace.SetFaceBool("isCribing", false);
            PlayerActor.instance.catFace.SetFaceBool("isRunning", true);
        }
        else
        {
            PlayerActor.instance.anim.SetBool("isDowning", false);
        }

        if (Mathf.Approximately(PlayerActor.instance.rigid.velocity.x, 0) || Input.GetAxis("Vertical") > 0.1f)
        {
            if ((!PlayerActor.instance.anim.GetBool("isDowning")))  //在空中转过后不再转回,需要加(!PlayerActor.instance.isJumpHoriz) 
            {
                //Debug.Log("纵跳");
                PlayerActor.instance.anim.SetBool("isJumping", true);
                PlayerActor.instance.anim.SetBool("isJumpingHoriz", false);
                PlayerActor.instance.anim.SetBool("isDowning", false);

                PlayerActor.instance.skin.ChangeCribeFace();

                PlayerActor.instance.catFace.SetFaceBool("isCribing", true);
                PlayerActor.instance.catFace.SetFaceBool("isRunning", false);
            }

        }
        else
        {
            //Debug.Log("横跳");
            PlayerActor.instance.anim.SetBool("isDowning", false);
            PlayerActor.instance.isJumpHoriz = true;
            PlayerActor.instance.anim.SetBool("isJumping", false);
            PlayerActor.instance.anim.SetBool("isJumpingHoriz", true);
            

            PlayerActor.instance.skin.ChangeNormalFace();

            PlayerActor.instance.catFace.SetFaceBool("isCribing", false);
            PlayerActor.instance.catFace.SetFaceBool("isRunning", true);
        }
    }

    private void PlayEffect()
    {
        if (Mathf.Approximately(PlayerActor.instance.rigid.velocity.x, 0) || Input.GetAxis("Vertical") > 0.1f)
        {
            /*if ((!PlayerActor.instance.anim.GetBool("isDowning")))  
            {
                //Debug.Log("纵跳");
                if (!hasPlayedEffect)
                {
                    hasPlayedEffect = true;
                    PlayerActor.instance.catEffect.PlayCharacterEffect(CharacterEffectType.Jump_1, true);
                    PlayerActor.instance.catEffect.PlayCharacterEffect(CharacterEffectType.Jump_2, true);
                }

            }*/

        }
        else
        {
            //Debug.Log("横跳");

            if (!hasPlayedEffect)
            {
                hasPlayedEffect = true;
                PlayerActor.instance.catEffect.PlayCharacterEffect(CharacterEffectType.Jump_1, true);
            }
        }
    }

    private void CheckLongTimeJump()
    {
        if (PlayerActor.instance.jumpTimer > 0)//起跳长按更高
        {
            PlayerActor.instance.rigid.velocity = 
                new Vector2(PlayerActor.instance.rigid.velocity.x, PlayerActor.instance.jumpForce);
            PlayerActor.instance.jumpTimer -= Time.deltaTime;
            if (Input.GetKeyUp(KeyCode.Space))
            {
                PlayerActor.instance.jumpTimer = 0;
            }
        }
    }

    private bool CheckAutoLand()
    {
        if ((PlayerActor.instance.isLeftHandCatch || PlayerActor.instance.isRightHandCatch)
            && PlayerActor.instance.rigid.velocity.y < -0.1f) // 手够地面
        {
            //检测点向下挪动0.1，防止检测到站立碰撞点 + Vector3.down * 0.1f
            //Ray2D ray = new Ray2D(PlayerActor.instance.transform.position + Vector3.down * 0.05f, PlayerActor.instance.rigid.velocity);
            Ray2D ray = new Ray2D(PlayerActor.instance.transform.position + Vector3.down * 0.05f, Vector2.down);

            Debug.DrawRay(ray.origin, ray.direction, Color.yellow);

            RaycastHit2D info = Physics2D.Raycast(ray.origin, ray.direction);

            if (info.collider != null)
            {
                Debug.Log("手抓的检测tag " + info.collider.tag);
                Debug.Log("手抓的检测name " + info.collider.gameObject.name);
                if (info.collider.CompareTag("Ground"))
                {
                    if (
                        info.point.y > (PlayerActor.instance.Left_Leg.position.y + PlayerActor.instance.Right_Leg.position.y) / 2 - 0.5f &&
                        info.point.y < (PlayerActor.instance.Left_Leg.position.y + PlayerActor.instance.Right_Leg.position.y) / 2 + 0.5f
                        )
                    {
                        return false;
                    }
                    else
                    {
                        Debug.Log("身体超出范围: 测试点y，腿y");
                        Debug.Log(info.point.y);
                        Debug.Log((PlayerActor.instance.Left_Leg.position.y + PlayerActor.instance.Right_Leg.position.y) / 2);
                        return true;
                    }

                    
                }
            }

            
        }

        return false;
    }
}

