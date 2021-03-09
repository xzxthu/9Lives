using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpState : ActorState
{
    private Actor _actor;
    public override ActorStateType StateType
    {
        get
        {
            return ActorStateType.Jump;
        }
    }

    public override void Enter(params object[] param)
    {
        //Debug.Log("JumpState Enter");
        _actor = param[0] as Actor;
        if (_actor != null)
        {
            PlayerActor.instance.anim.SetBool("isWalking", false);
            PlayerActor.instance.anim.SetBool("isRunning", false);
        }


    }

    public override void FixedUpdate()
    {
        CalculateSpeed();
        PlayerActor.instance.UpdateInActor();
        PlayAnimation();

        if (PlayerActor.instance.isGround)
        {
            PlayerActor.instance.TransState(ActorStateType.Idle);
            return;
        }

        if (CheckAutoLand())
        {
            Level_2_Manager.instance.isHanging = true;
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
        //Debug.Log("JumpState Exit");

    }

    private void CalculateSpeed()
    {
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
                new Vector2(PlayerActor.instance.moveInput * PlayerActor.instance.speed,
                PlayerActor.instance.rigid.velocity.y);
        }
        else//Decelerate
        {
            if (PlayerActor.instance.speed > 0)
            {
                if (Input.GetAxis("Vertical") > 0.5f) //Accelerate
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

            PlayerActor.instance.rigid.velocity =
                new Vector2(PlayerActor.instance.recordMoveInput * PlayerActor.instance.speed,
                PlayerActor.instance.rigid.velocity.y);
        }
    }

    private void PlayAnimation()
    {
        if (!PlayerActor.instance.anim.GetBool("isJumping") && !PlayerActor.instance.anim.GetBool("isJumpingHoriz"))//没有任何输入的情况下的向下动画
        {
            PlayerActor.instance.anim.SetBool("isDowning", true);
        }
        else
        {
            PlayerActor.instance.anim.SetBool("isDowning", false);
        }

        if (Mathf.Approximately(PlayerActor.instance.moveInput, 0))
        {
            if ((!PlayerActor.instance.isHoriz || Input.GetAxis("Vertical") > 0.5f) && !PlayerActor.instance.anim.GetBool("isDowning"))  //在空中转过后不再转回,除非按上
            {
                PlayerActor.instance.anim.SetBool("isJumping", true);
                PlayerActor.instance.anim.SetBool("isJumpingHoriz", false);
                PlayerActor.instance.anim.SetBool("isDowning", false);
            }

        }
        else
        {
            PlayerActor.instance.isHoriz = true;
            PlayerActor.instance.anim.SetBool("isJumping", false);
            PlayerActor.instance.anim.SetBool("isJumpingHoriz", true);
            PlayerActor.instance.anim.SetBool("isDowning", false);
        }


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
            PlayerActor.instance.isHoriz = false;

        }

        
    }

    private void CheckLongTimeJump()
    {
        if (PlayerActor.instance.jumpTimer > 0)//起跳长按更高
        {
            PlayerActor.instance.rigid.velocity = Vector2.up * PlayerActor.instance.jumpForce;
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
            && PlayerActor.instance.rigid.velocity.y < 0) // 手够地面
        {

            Ray2D ray = new Ray2D(PlayerActor.instance.transform.position, PlayerActor.instance.rigid.velocity);

            //Debug.DrawRay(ray.origin, ray.direction, Color.yellow);

            RaycastHit2D info = Physics2D.Raycast(ray.origin, ray.direction);

            if (info.collider != null)
            {//如果发生了碰撞
                if (info.collider.CompareTag("Ground"))
                {
                    if (
                        info.collider.gameObject.transform.position.y > (PlayerActor.instance.Left_Leg.position.y + PlayerActor.instance.Right_Leg.position.y) / 2 - 0.5f &&
                        info.collider.gameObject.transform.position.y < (PlayerActor.instance.Left_Leg.position.y + PlayerActor.instance.Right_Leg.position.y) / 2 + 0.5f
                        )
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        return false;
    }
}

