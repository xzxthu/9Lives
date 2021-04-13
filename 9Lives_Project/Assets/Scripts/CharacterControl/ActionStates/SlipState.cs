using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlipState : ActorState
{
    private float direction;

    private Actor _actor;
    public override ActorStateType StateType
    {
        get
        {
            return ActorStateType.Slip;
        }
    }


    public override void Enter(params object[] param)
    {
        Debug.Log("SlipState Enter");
        _actor = param[0] as Actor;
        if (_actor != null)
        {
            /*if(Mathf.Approximately( PlayerActor.instance.rigid.velocity.x,0))
            {
                PlayerActor.instance.TransState(ActorStateType.Idle);
                return;
            }*/

            PlayerActor.instance.anim.SetBool("isWalking", false);
            PlayerActor.instance.anim.SetBool("isRunning", false);
            PlayerActor.instance.anim.SetBool("isJumping", false);
            PlayerActor.instance.anim.SetBool("isJumpingHoriz", false);
            PlayerActor.instance.anim.SetBool("isDowning", false);
            PlayerActor.instance.isJumpHoriz = false;
            PlayerActor.instance.jumpTimer = 0;
            PlayerActor.instance.isHurted = false;

            PlayerActor.instance.isSlipping = false;

            PlayerActor.instance.anim.SetBool("isSlipping", true);


            PlayerActor.instance.speed = PlayerActor.instance.slippingSpeed;

            
            direction = PlayerActor.instance.transform.localScale.x > 0 ? 1f : -1f;
            PlayerActor.instance.rigid.velocity =
            new Vector2(direction * PlayerActor.instance.speed, PlayerActor.instance.rigid.velocity.y);
        }

        PlayerActor.instance.catFace.SetFaceBool("isClosingEyes", true);
    }


    public override void FixedUpdate()
    {
        Slipping();
        CheckJump();

        if (PlayerActor.instance.isHurted)
        {
            PlayerActor.instance.TransState(ActorStateType.Hurted);
            return;
        }

        if (!PlayerActor.instance.isGround)
        {
            PlayerActor.instance.TransState(ActorStateType.Jump);
            return;
        }

        if (Mathf.Approximately(PlayerActor.instance.rigid.velocity.x, 0))
        {
            PlayerActor.instance.TransState(ActorStateType.Idle);
            return;
        }
    }

    public override void Update()
    {

    }


    public override void Exit()
    {
        _actor = null;
        PlayerActor.instance.anim.SetBool("isSlipping", false);
        //Debug.Log("IdleState Exit");
        PlayerActor.instance.catFace.SetFaceBool("isClosingEyes", false);
    }


    private void Slipping()
    {
        if (PlayerActor.instance.speed > 0)
        {
            PlayerActor.instance.speed -= Time.fixedDeltaTime * PlayerActor.instance.decelerateWhenSlipping;
        }
        else
        {
            PlayerActor.instance.speed = 0;
            PlayerActor.instance.recordMoveInput = 0;
        }

        PlayerActor.instance.rigid.velocity =
            new Vector2(direction * PlayerActor.instance.speed,PlayerActor.instance.rigid.velocity.y);

    }

    private void CheckJump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {

            PlayerActor.instance.rigid.velocity = Vector2.up * PlayerActor.instance.jumpForce;
            PlayerActor.instance.jumpTimer = PlayerActor.instance.jumpHeightTime;

            PlayerActor.instance.anim.SetBool("isJumping", false);
            PlayerActor.instance.anim.SetBool("isJumpingHoriz", true);
            PlayerActor.instance.anim.SetBool("isWalking", false);
            PlayerActor.instance.anim.SetBool("isRunning", false);
            PlayerActor.instance.isJumpHoriz = true; //横跳

            PlayerActor.instance.anim.SetBool("isSlipping", false);

        }
    }
}
