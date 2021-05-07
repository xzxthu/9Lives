using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : ActorState
{

    private Actor _actor;
    private bool hasPlayDec;
    private bool needDec;
    private float timer = 0f;

    public override ActorStateType StateType
    {
        get
        {
            return ActorStateType.Idle;
        }
    }

    public override void Enter(params object[] param)
    {
        Debug.Log("IdleState Enter");
        _actor = param[0] as Actor;
        if (_actor != null)
        {
            
            PlayerActor.instance.anim.SetBool("isWalking", false);
            PlayerActor.instance.anim.SetBool("isRunning", false);

        }

        PlayerActor.instance.needIK = true;

        hasPlayDec = false;
        timer = 0;
        if (Mathf.Abs(PlayerActor.instance.rigid.velocity.x) > PlayerActor.instance.maxRunSpeed * 0.85f)
        {
            PlayerActor.instance.anim.SetTrigger("RunningStopTrigger");
            needDec = true;
        }
        else
        {
            needDec = false;
        }
        //PlayerActor.instance.catFace.SetFaceBool("isRunning", false);
        //PlayerActor.instance.catFace.SetFaceBool("isRunning", false);

    }

    public override void FixedUpdate()
    {
        if (LevelManager.instance.dontMove) return;

        Decelerate();
        PlayerActor.instance.UpdateInActor();

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

        if (!Mathf.Approximately(PlayerActor.instance.moveInput,0) && PlayerActor.instance.isGround)
        {
            PlayerActor.instance.TransState(ActorStateType.Run);
            return;
        }
        if (!PlayerActor.instance.isGround)
        {
            PlayerActor.instance.TransState(ActorStateType.Jump);
            return;
        }
    }
    public override void Update()
    {
        if (LevelManager.instance.dontMove) return;

        PlayAnimation();
        CheckJump();
        PlayerActor.instance.CheckAutoDown();

        timer += Time.deltaTime;
    }


    public override void Exit()
    {
        _actor = null;

        PlayerActor.instance.needIK = false;

        //Debug.Log("IdleState Exit");
    }

    private void Decelerate()
    {
        
        if (PlayerActor.instance.speed > 0)
        {
            PlayerActor.instance.speed -= Time.fixedDeltaTime * PlayerActor.instance.decelerateOnGround;
        }
        else
        {
            PlayerActor.instance.speed = 0;
            PlayerActor.instance.recordMoveInput = 0;
        }
        
        PlayerActor.instance.rigid.velocity =
            new Vector2(PlayerActor.instance.recordMoveInput * PlayerActor.instance.speed,
            PlayerActor.instance.rigid.velocity.y);

        if (needDec && !hasPlayDec && timer>0.075f) //奔跑到急停时才播放急停动画,防止往返跑出急停
        {
            hasPlayDec = true;
            
            PlayerActor.instance.catEffect.PlayCharacterEffect(CharacterEffectType.Decelerate);
        }
    }

    private void PlayAnimation()
    {

        

        if ((PlayerActor.instance.anim.GetBool("isJumping") || PlayerActor.instance.anim.GetBool("isJumpingHoriz") ||
                PlayerActor.instance.anim.GetBool("isDowning")) && PlayerActor.instance.rigid.velocity.y == 0) //结束动画,跳的过程中碰到地块则不变
        {
            //Debug.Log(rigid.velocity.y);
            if (PlayerActor.instance.rigid.velocity.y < PlayerActor.instance.speedForHighLandding) //控制是否大落地(至少要2级台阶)
            {
                PlayerActor.instance.anim.SetBool("isLandingFromHigh", true);
            }
            else
            {
                PlayerActor.instance.anim.SetBool("isLandingFromHigh", false);
            }
            PlayerActor.instance.anim.SetBool("isJumping", false);
            PlayerActor.instance.anim.SetBool("isJumpingHoriz", false);
            PlayerActor.instance.anim.SetBool("isDowning", false);
            PlayerActor.instance.isJumpHoriz = false;

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
                MusicManager.GetInstance().PlayBGM("Jump1");

                PlayerActor.instance.catEffect.PlayCharacterEffect(CharacterEffectType.Jump_1, true);
                PlayerActor.instance.catEffect.PlayCharacterEffect(CharacterEffectType.Jump_2, true);

                PlayerActor.instance.catFace.SetFaceBool("isCribing", true);
            }
        }

    }
}
