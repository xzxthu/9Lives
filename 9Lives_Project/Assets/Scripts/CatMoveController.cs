using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatMoveController : MonoBehaviour
{
    public float maxWalkSpeed = 0.5f; //最大速度
    public float maxRunSpeed = 1f; //最大速度
    public float accelerate = 1f; //加速
    public float decelerateInGround = 1f;  //在地面上才横向减速（空中不减速）
    public float decelerateInAir = 1f; //
    public float changeDirectionDecelerate = 2f; //转向会横向减速
    public float speedForHighLandding = -15; //大落地动作的下坠速度

    public Transform feetPos;
    public float feetCheckRadius;
    public Transform Left_Leg;
    public Transform Right_Leg;
    public float handCheckRadius;
    public Transform Left_BackLeg;
    public Transform Right_BackLeg;

    public float jumpForce;
    public float jumpHeightTime;

    private LayerMask ground;
    private Rigidbody2D rigid;
    private Animator anim;

    private float moveInput;
    private float recordMoveInput; //记录上一帧输入，用于计算转向减速
    private float speed = 0f;
    private float jumpTimer;
    private float acTimer;
    private float horizontal;

    private bool isGround;
    private bool isLeftHandCatch = false;
    private bool isRightHandCatch = false;
    private bool isTurnAround = false;
    private bool isJumpClimbSuccess = false;
    private bool isHoriz = false;
    public bool isHunging = false;

    private void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        ground = LayerMask.GetMask("Ground");
        anim = GetComponentInChildren<Animator>();
        horizontal = transform.localScale.x;

    }

    private void FixedUpdate()
    {
        float updateFeetX = (Left_BackLeg.position.x + Right_BackLeg.position.x) * 0.5f;
        feetPos.position = new Vector3(updateFeetX, feetPos.position.y, feetPos.position.z); //根据后脚位置更新


        moveInput = Input.GetAxisRaw("Horizontal");

        if (moveInput > 0.1f)//左右镜像
        {
            transform.localScale = new Vector3(horizontal, transform.localScale.y, transform.localScale.z);
        }
        if (moveInput < -0.1f)
        {
            transform.localScale = new Vector3(-horizontal, transform.localScale.y, transform.localScale.z);
        }


        if (speed > maxRunSpeed * 0.75) //急转向
        {
            if (moveInput * recordMoveInput < 0) isTurnAround = true;
        }
        else
        {
            isTurnAround = false;
        }

        if(!isHunging)
        {
            if (moveInput != 0)//有输入则加速
            {
                recordMoveInput = moveInput; //更新每帧记录

                float maxSpeed = Input.GetKey(KeyCode.LeftShift) ? maxWalkSpeed : maxRunSpeed;

                if (speed < maxSpeed)
                {
                    if (isGround) //在地上
                    {
                        speed += Time.fixedDeltaTime * accelerate;

                        if (isTurnAround)
                        {
                            speed /= changeDirectionDecelerate;  //转向减速
                            isTurnAround = false;
                        }
                    }
                    else //在空中
                    {
                        if (Input.GetAxis("Vertical") > 0.5f) //在空中按上
                        {
                            speed *= 0.5f;
                        }
                        else
                        {
                            speed = maxRunSpeed;
                        }

                    }
                }
                else
                {
                    speed = maxSpeed;
                }

                rigid.velocity = new Vector2(moveInput * speed, rigid.velocity.y); //最后计算得到的速度
            }
            else//无输入减速（急停）
            {
                ////SoundManager.instance.StopSound();

                //Debug.Log("no input");

                if (speed > 0)
                {
                    if (isGround)
                    {
                        speed -= Time.fixedDeltaTime * decelerateInGround;
                    }
                    else //在空中
                    {
                        if (Input.GetAxis("Vertical") > 0.5f) //在空中按上急停
                        {
                            speed -= Time.fixedDeltaTime * decelerateInAir * 2;
                        }
                        else
                        {
                            speed -= Time.fixedDeltaTime * decelerateInAir;
                        }

                    }

                }
                else
                {
                    speed = 0;
                    recordMoveInput = 0;
                }

                rigid.velocity = new Vector2(recordMoveInput * speed, rigid.velocity.y);
            }

            isGround = Physics2D.OverlapCircle(feetPos.position, feetCheckRadius, ground); //判断是否在地面，通过画圆产生按键缓存
            isLeftHandCatch = Physics2D.OverlapCircle(Left_Leg.position, handCheckRadius, ground); //判断左右手
            isRightHandCatch = Physics2D.OverlapCircle(Right_Leg.position, handCheckRadius, ground);



            //动画控制
            if (!isGround) //空中
            {
                //重置
                anim.SetBool("isWalking", false);
                anim.SetBool("isRunning", false);

                if(!anim.GetBool("isJumping")&& !anim.GetBool("isJumpingHoriz"))//没有任何输入的情况下的向下动画
                {
                    anim.SetBool("isDowning", true);
                }
                else
                {
                    anim.SetBool("isDowning", false);
                }

                if (moveInput == 0)
                {
                    if ((!isHoriz || Input.GetAxis("Vertical") > 0.5f)&& !anim.GetBool("isDowning"))  //在空中转过后不再转回,除非按上
                    {
                        anim.SetBool("isJumping", true);
                        anim.SetBool("isJumpingHoriz", false);
                        anim.SetBool("isDowning", false);
                    }

                }
                else
                {
                    isHoriz = true;
                    anim.SetBool("isJumping", false);
                    anim.SetBool("isJumpingHoriz", true);
                    anim.SetBool("isDowning", false);
                }

            }
            else //地面
            {
                if ((anim.GetBool("isJumping") || anim.GetBool("isJumpingHoriz")|| anim.GetBool("isDowning")) && rigid.velocity.y==0) //结束动画,跳的过程中碰到地块则不变
                {
                    //Debug.Log(rigid.velocity.y);
                    if (rigid.velocity.y < speedForHighLandding) //控制是否大落地(至少要2级台阶)
                    {
                        anim.SetBool("isLandingFromHigh", true);
                    }
                    else
                    {
                        anim.SetBool("isLandingFromHigh", false);
                    }
                    anim.SetBool("isJumping", false);
                    anim.SetBool("isJumpingHoriz", false);
                    anim.SetBool("isDowning",false);
                    isHoriz = false;

                }

                if (rigid.velocity.y == 0)
                {
                    if (moveInput != 0) //走路&跑步
                    {
                        if (Input.GetKey(KeyCode.LeftShift))
                        {
                            anim.SetBool("isWalking", true);
                            anim.SetBool("isRunning", false);
                            
                        }
                        else
                        {
                            anim.SetBool("isWalking", false);
                            anim.SetBool("isRunning", true);
                        }

                    }
                    else
                    {
                        if ((Mathf.Abs(rigid.velocity.x) > maxRunSpeed * 0.75f) && rigid.velocity.y == 0) //奔跑到急停时才播放急停动画
                        {
                            anim.SetTrigger("RunningStopTrigger");
                        }
                        anim.SetBool("isWalking", false);
                        anim.SetBool("isRunning", false);

                    }
                }
                
            }

            if (isGround && Input.GetKeyDown(KeyCode.Space))
            {
                rigid.velocity = Vector2.up * jumpForce;
                jumpTimer = jumpHeightTime;

                if (moveInput == 0)
                {
                    anim.SetBool("isJumping", true);
                    anim.SetBool("isJumpingHoriz", false);
                    anim.SetBool("isWalking", false);
                    anim.SetBool("isRunning", false);
                }
                else
                {
                    //isHoriz = true;
                    //anim.SetBool("isJumping", false);
                    //anim.SetBool("isJumpingHoriz", true);
                }
            }


        if (jumpTimer > 0)//起跳长按更高
        {
            rigid.velocity = Vector2.up * jumpForce;
            jumpTimer -= Time.deltaTime;
            if (Input.GetKeyUp(KeyCode.Space))
            {
                jumpTimer = 0;
            }
        }



            if ((isLeftHandCatch || isRightHandCatch) && !isGround && rigid.velocity.y<0) // 手够地面
            {
                CheckAutoLand();


            }
            
            if(!isLeftHandCatch && !isRightHandCatch &&isGround && rigid.velocity.y==0 && !anim.GetBool("isJumping")) //平的时候手出地面范围了，无输入的时候不判定
            {
                
                if (moveInput != 0)
                {
                    //Debug.Log("HandOut!");
                    CheckAutoDown();
                    //Invoke("CheckAutoDown", 0.5f); //等待
                }
                
                //CheckAutoDown();

                
            }

        }
        else //挂住的时候
        {
            StartCoroutine(ResetHunging());
        }
        

        


    }

    private void Update()
    {
        if (isHunging) return;

        if (isGround && Input.GetKeyDown(KeyCode.Space))
        {

            //SoundManager.instance.PlayJumpSound();

            rigid.velocity = Vector2.up * jumpForce;
            jumpTimer = jumpHeightTime;

            if (moveInput == 0)
            {
                anim.SetBool("isJumping", true);
                anim.SetBool("isJumpingHoriz", false);
                anim.SetBool("isWalking", false);
                anim.SetBool("isRunning", false);
            }
            else
            {
                //isHoriz = true;
                //anim.SetBool("isJumping", false);
                //anim.SetBool("isJumpingHoriz", true);
            }
        }


        if (jumpTimer > 0)//起跳长按更高
        {
            rigid.velocity = Vector2.up * jumpForce;
            jumpTimer -= Time.deltaTime;
            if (Input.GetKeyUp(KeyCode.Space))
            {
                jumpTimer = 0;
            }
        }


    }





    private void CheckAutoLand() //判断是那种落地方式

    {

        Ray2D ray = new Ray2D(transform.position, rigid.velocity);

        //Debug.DrawRay(ray.origin, ray.direction, Color.yellow);

        RaycastHit2D info = Physics2D.Raycast(ray.origin, ray.direction);

        if (info.collider != null)
        {//如果发生了碰撞
            if (info.collider.CompareTag("Ground"))
            {
                if (
                    info.collider.gameObject.transform.position.y > (Left_Leg.position.y + Right_Leg.position.y) / 2 - 0.5f &&
                    info.collider.gameObject.transform.position.y < (Left_Leg.position.y + Right_Leg.position.y) / 2 + 0.5f
                    )
                {
                    return;
                }
            }
        }

        isHunging = true;
        speed = 0;

        float notKeepDropping;//防止挂不住反复纵跳，给一个位移
        notKeepDropping = Left_Leg.position.x - transform.position.x;
        float moveOffetY = 0;

        if (notKeepDropping > -0.3f) //垂直挂的时候添加横向位移
        {
            
            Ray2D rayUpLeft = new Ray2D(Left_Leg.position + Vector3.up * 0.2f, Vector2.down);
            Ray2D rayUpRight = new Ray2D(Right_Leg.position + Vector3.up * 0.2f, Vector2.down);

            RaycastHit2D infoLeft = Physics2D.Raycast(rayUpLeft.origin, Vector2.down, 0.5f,ground);
            RaycastHit2D infoRight = Physics2D.Raycast(rayUpRight.origin, Vector2.down, 0.5f, ground);


            if(infoLeft.collider!=null)
            {
                notKeepDropping = infoLeft.collider.gameObject.transform.position.x - transform.position.x;
            }


            if (infoRight.collider != null)
            {
                if(infoLeft.collider != null)
                {
                    if (infoLeft.collider.gameObject.transform.position.y > infoRight.collider.gameObject.transform.position.y)
                    {
                        notKeepDropping = infoRight.collider.gameObject.transform.position.x - transform.position.x;
                        
                    }
                }
                else
                {
                    notKeepDropping = infoRight.collider.gameObject.transform.position.x - transform.position.x;
                }
                
            }

            notKeepDropping *= 0.4f;
        }
        else 
        {
            Ray2D rayUpLeft = new Ray2D(Left_Leg.position + Vector3.up * 0.2f, Vector2.down);
            Ray2D rayUpRight = new Ray2D(Right_Leg.position + Vector3.up * 0.2f, Vector2.down);

            RaycastHit2D infoLeft = Physics2D.Raycast(rayUpLeft.origin, Vector2.down, 0.5f, ground);
            RaycastHit2D infoRight = Physics2D.Raycast(rayUpRight.origin, Vector2.down, 0.5f, ground);


            if (infoLeft.collider != null && isLeftHandCatch) //防止没抓住的手误触
            {
                notKeepDropping = infoLeft.collider.gameObject.transform.position.x - Left_Leg.position.x;
            }


            if (infoRight.collider != null && isRightHandCatch)
            {
                if (infoLeft.collider != null && isLeftHandCatch)
                {
                    if (infoLeft.collider.gameObject.transform.position.y > infoRight.collider.gameObject.transform.position.y)
                    {
                        notKeepDropping = infoRight.collider.gameObject.transform.position.x - Right_Leg.position.x;

                    }
                }
                else
                {
                    notKeepDropping = infoRight.collider.gameObject.transform.position.x - Right_Leg.position.x;
                }

            }

            moveOffetY = 0.6f;
        }

        //Debug.Log(notKeepDropping);

        Vector3 moveOffet = new Vector3(notKeepDropping * 0.4f, moveOffetY, 0); //横向和向上移动一点
        transform.position = (Left_Leg.position + Right_Leg.position) * 0.5f + moveOffet;
        anim.SetBool("isJumping", false);
        anim.SetBool("isJumpingHoriz", false);
        anim.SetBool("isDowning", false);
        isHoriz = false;

        if(Mathf.Abs(rigid.velocity.x)>maxRunSpeed*0.75) //是否播放吊着的摇晃
        {
            anim.SetBool("isHangingHoriz", true);
        }
        else
        {
            anim.SetBool("isHangingHoriz", false);
        }

        anim.SetTrigger("HangingTrigger");
        PlayerActor.instance.isHanging = true;
        rigid.velocity = Vector2.zero;

    }

    private void CheckAutoDown() //判断自动down one way
    {
        float offsetX = (transform.position.x - Left_Leg.position.x) * 1f * (Mathf.Abs(rigid.velocity.x)/maxRunSpeed); //往身体方向挪一点，速度越大越远
        Vector3 offset = new Vector3(offsetX, 0.5f, 0);
        //Debug.Log(offset.x);

        //offset = Vector3.zero;
        Ray2D rayUpLeft = new Ray2D(Left_Leg.position + offset, Vector2.down);
        Ray2D rayUpRight = new Ray2D(Right_Leg.position + offset, Vector2.down);

        RaycastHit2D infoLeft = Physics2D.Raycast(rayUpLeft.origin, Vector2.down,1.5f, ground);
        RaycastHit2D infoRight = Physics2D.Raycast(rayUpRight.origin, Vector2.down, 1.5f, ground);

        //Debug.DrawRay(rayUpLeft.origin, Vector2.down, Color.yellow);
        //Debug.DrawRay(rayUpRight.origin, Vector2.down, Color.yellow);

        if (infoLeft.collider == null && infoRight.collider == null ) 
        {
            Level_2_Manager.instance.isDown = true;
            //Debug.Log("Null2");
        }

        /*if(!Physics2D.OverlapCircle(Left_Leg.position, 1f, ground)&& !Physics2D.OverlapCircle(Right_Leg.position, 1f, ground))
        {
            Level_2_Manager.instance.isDown = true;
        }*/

    }
    public IEnumerator ResetHunging()
    {
        yield return new WaitForSeconds(0.9f);
        isHunging = false;
        //Level_2_Manager.instance.isHanging = false;
    }

}


