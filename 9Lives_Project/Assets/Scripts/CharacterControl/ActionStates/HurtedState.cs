﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtedState : ActorState
{
    private float timer;

    private Actor _actor;
    public override ActorStateType StateType
    {
        get
        {
            return ActorStateType.Hurted;
        }
    }

    public override void Enter(params object[] param)
    {
        //Debug.Log("HurtedState Enter");
        _actor = param[0] as Actor;
        if (_actor != null)
        {
            PlayerActor.instance.anim.SetBool("isWalking", false);
            PlayerActor.instance.anim.SetBool("isRunning", false);
            PlayerActor.instance.anim.SetBool("isJumping", false);
            PlayerActor.instance.anim.SetBool("isJumpingHoriz", false);
            PlayerActor.instance.anim.SetBool("isDowning", false);
            PlayerActor.instance.isHoriz = false;
            PlayerActor.instance.jumpTimer = 0;

            Level_2_Manager.instance.isHurted = false;

            timer = Level_2_Manager.instance.hurtTime;
            PlayerActor.instance.anim.SetBool("isHurted", true);
            HurtedMove();
        }


    }

    public override void FixedUpdate()
    {
        WaitForExit();
    }

    public override void Update()
    {

    }

    public override void Exit()
    {
        _actor = null;
        // add animation control here
        PlayerActor.instance.anim.SetBool("isHurted", false);
        PlayerActor.instance.speed = Mathf.Abs(PlayerActor.instance.rigid.velocity.x);
        //Debug.Log("HurtedState Exit");
    }

    private void WaitForExit()
    {
        if(timer>0)
        {
            timer -= Time.fixedDeltaTime;
        }
        else
        {
            PlayerActor.instance.TransState(ActorStateType.Idle);
        }
    }

    private void HurtedMove()
    {
        PlayerActor.instance.rigid.velocity = Level_2_Manager.instance.hurtDir * Level_2_Manager.instance.hurtForce;
    }


}