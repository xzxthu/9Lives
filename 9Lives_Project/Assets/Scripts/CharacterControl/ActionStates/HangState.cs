using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HangState : ActorState
{
    private Actor _actor;
    public override ActorStateType StateType
    {
        get
        {
            return ActorStateType.Hang;
        }
    }

    public override void Enter(params object[] param)
    {
        Debug.Log("HangState Enter");
        _actor = param[0] as Actor;
        if (_actor != null)
        {

            MoveWhenHang();

            PlayerActor.instance.anim.SetBool("isJumping", false);
            PlayerActor.instance.anim.SetBool("isJumpingHoriz", false);
            PlayerActor.instance.anim.SetBool("isDowning", false);
            PlayerActor.instance.isHoriz = false;
            if (Mathf.Abs(PlayerActor.instance.rigid.velocity.x) > PlayerActor.instance.maxRunSpeed * 0.75) //是否播放吊着的摇晃
            {
                PlayerActor.instance.anim.SetBool("isHangingHoriz", true);
            }
            else
            {
                PlayerActor.instance.anim.SetBool("isHangingHoriz", false);
            }

            PlayerActor.instance.anim.SetTrigger("HangingTrigger");

        }


    }

    public override void FixedUpdate()
    {
        PlayerActor.instance.UpdateInActor();

        if (Level_2_Manager.instance.isHurted)
        {
            PlayerActor.instance.TransState(ActorStateType.Hurted);
            return;
        }

        if (!Level_2_Manager.instance.isHanging)//|| (!PlayerActor.instance.isLeftHandCatch&& !PlayerActor.instance.isRightHandCatch)
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
        // add animation control here
        Level_2_Manager.instance.isHanging = false;
        //Debug.Log("HangState Exit");
    }

    private void MoveWhenHang()
    {
        PlayerActor.instance.speed = 0;
        PlayerActor.instance.rigid.velocity = Vector2.zero;


        float notKeepDropping;//防止挂不住反复纵跳，给一个位移
        notKeepDropping = PlayerActor.instance.Left_Leg.position.x - PlayerActor.instance.transform.position.x;
        float moveOffetY = 0;

        if (notKeepDropping > -0.3f) //垂直挂的时候添加横向位移
        {
            Debug.Log("垂直挂住");
            Ray2D rayUpLeft = new Ray2D(PlayerActor.instance.Left_Leg.position + Vector3.up * 0.2f, Vector2.down);
            Ray2D rayUpRight = new Ray2D(PlayerActor.instance.Right_Leg.position + Vector3.up * 0.2f, Vector2.down);

            RaycastHit2D infoLeft = Physics2D.Raycast(rayUpLeft.origin, Vector2.down, 0.5f, PlayerActor.instance.ground);
            RaycastHit2D infoRight = Physics2D.Raycast(rayUpRight.origin, Vector2.down, 0.5f, PlayerActor.instance.ground);


            if (infoLeft.collider != null)
            {
                notKeepDropping = infoLeft.collider.gameObject.transform.position.x - PlayerActor.instance.transform.position.x;
            }


            if (infoRight.collider != null)
            {
                if (infoLeft.collider != null)
                {
                    if (infoLeft.collider.gameObject.transform.position.y > infoRight.collider.gameObject.transform.position.y)
                    {
                        notKeepDropping = infoRight.collider.gameObject.transform.position.x - PlayerActor.instance.transform.position.x;

                    }
                }
                else
                {
                    notKeepDropping = infoRight.collider.gameObject.transform.position.x - PlayerActor.instance.transform.position.x;
                }

            }

            notKeepDropping *= 0.4f;
        }
        else //横着挂住
        {
            Ray2D rayUpLeft = new Ray2D(PlayerActor.instance.Left_Leg.position + Vector3.up * 0.2f, Vector2.down);
            Ray2D rayUpRight = new Ray2D(PlayerActor.instance.Right_Leg.position + Vector3.up * 0.2f, Vector2.down);

            RaycastHit2D infoLeft = Physics2D.Raycast(rayUpLeft.origin, Vector2.down, 0.5f, PlayerActor.instance.ground);
            RaycastHit2D infoRight = Physics2D.Raycast(rayUpRight.origin, Vector2.down, 0.5f, PlayerActor.instance.ground);


            if (infoLeft.collider != null && PlayerActor.instance.isLeftHandCatch) //防止没抓住的手误触
            {
                notKeepDropping = infoLeft.collider.gameObject.transform.position.x - PlayerActor.instance.Left_Leg.position.x;
            }


            if (infoRight.collider != null && PlayerActor.instance.isRightHandCatch)
            {
                if (infoLeft.collider != null && PlayerActor.instance.isLeftHandCatch)
                {
                    if (infoLeft.collider.gameObject.transform.position.y > infoRight.collider.gameObject.transform.position.y)
                    {
                        notKeepDropping = infoRight.collider.gameObject.transform.position.x - PlayerActor.instance.Right_Leg.position.x;

                    }
                }
                else
                {
                    notKeepDropping = infoRight.collider.gameObject.transform.position.x - PlayerActor.instance.Right_Leg.position.x;
                }

            }

            moveOffetY = 0.6f;
        }

        Vector3 moveOffet = new Vector3(notKeepDropping * 0.4f, moveOffetY, 0); //横向和向上移动一点
        PlayerActor.instance.transform.position = (PlayerActor.instance.Left_Leg.position + PlayerActor.instance.Right_Leg.position) * 0.5f + moveOffet;
    
}
}
