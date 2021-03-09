using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunState : ActorState
{
    private Actor _actor;
    public override ActorStateType StateType
    {
        get
        {
            return ActorStateType.Run;
        }
    }

    public override void Enter(params object[] param)
    {
        //Debug.Log("RunState Enter");
        _actor = param[0] as Actor;
        if (_actor != null)
        {
            PlayerActor.instance.anim.SetBool("isJumping", false);
            PlayerActor.instance.anim.SetBool("isJumpingHoriz", false);
            PlayerActor.instance.anim.SetBool("isDowning", false);


        }


    }

    public override void FixedUpdate()
    {
        CalculateSpeed();
        CalculateTurning();
        PlayerActor.instance.UpdateInActor();
        PlayAniamtion();
        PlayerActor.instance.CheckAutoDown();

        if (Level_2_Manager.instance.isHurted)
        {
            PlayerActor.instance.TransState(ActorStateType.Hurted);
            return;
        }

        if (Mathf.Approximately(PlayerActor.instance.moveInput, 0) && PlayerActor.instance.isGround)
        {
            PlayerActor.instance.TransState(ActorStateType.Idle);
            return;
        }

        if(!PlayerActor.instance.isGround)
        {
            PlayerActor.instance.TransState(ActorStateType.Jump);
            return;
        }
    }

    public override void Update()
    {
        CheckJump();
    }

    public override void Exit()
    {
        _actor = null;
        //Debug.Log("RunState Exit");

    }

    private void CalculateTurning()
    {
        if (PlayerActor.instance.speed > PlayerActor.instance.maxRunSpeed * 0.75) //Turning
        {
            if (PlayerActor.instance.moveInput * PlayerActor.instance.recordMoveInput < 0)
                PlayerActor.instance.isTurnAround = true;
        }
        else
        {
            PlayerActor.instance.isTurnAround = false;
        }

        PlayerActor.instance.recordMoveInput = PlayerActor.instance.moveInput; //record for calculate turning

    }

    private void CalculateSpeed()
    {
        float maxSpeed = Input.GetKey(KeyCode.LeftShift) ? PlayerActor.instance.maxWalkSpeed : PlayerActor.instance.maxRunSpeed;

        if (PlayerActor.instance.speed < maxSpeed)
        {
            PlayerActor.instance.speed += Time.fixedDeltaTime * PlayerActor.instance.accelerate;

            if (PlayerActor.instance.isTurnAround)
            {
                PlayerActor.instance.speed /= PlayerActor.instance.changeDirectionDecelerate;  //decelerate when turning
                PlayerActor.instance.isTurnAround = false;
            }
        }
        else
        {
            PlayerActor.instance.speed = maxSpeed;
        }

        PlayerActor.instance.rigid.velocity =
            new Vector2(PlayerActor.instance.moveInput * PlayerActor.instance.speed,
            PlayerActor.instance.rigid.velocity.y); //move cat
    }

    private void PlayAniamtion()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            PlayerActor.instance.anim.SetBool("isWalking", true);
            PlayerActor.instance.anim.SetBool("isRunning", false);

        }
        else
        {
            PlayerActor.instance.anim.SetBool("isWalking", false);
            PlayerActor.instance.anim.SetBool("isRunning", true);
        }
    }

    private void CheckJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            PlayerActor.instance.rigid.velocity = Vector2.up * PlayerActor.instance.jumpForce;
            PlayerActor.instance.jumpTimer = PlayerActor.instance.jumpHeightTime;
            if (Mathf.Approximately(PlayerActor.instance.moveInput, 0))//直接播垂直跳动画
            {
                PlayerActor.instance.anim.SetBool("isJumping", true);
                PlayerActor.instance.anim.SetBool("isJumpingHoriz", false);
                PlayerActor.instance.anim.SetBool("isWalking", false);
                PlayerActor.instance.anim.SetBool("isRunning", false);
            }
        }
    }

    
}
