using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Prototype
{
    public class CatController : MonoBehaviour
    {
        public float maxSpeed = 1f;
        public float accelerate = 1f;
        public float decelerateInGround = 1f;
        public float decelerateInAir = 1f;
        public float changeDirectionDecelerate = 2f; //转向减速
        public Transform feetPos;
        public float feetCheckRadius;
        public Transform jumpHandPos;
        public float handCheckRadius;
        public float jumpForce;
        public float jumpHeightTime;
        public Vector2 handClimbSpeed = new Vector2(1f, -4f);//能使用手登上去的速度

        private LayerMask ground;
        private Rigidbody2D rigid;
        private Animator anim;
        private float moveInput;
        private float recordMoveInput;
        private float speed = 0f;
        private float jumpTimer;
        private float acTimer;
        private float horizontal;


        private bool isGround;
        private bool isTurnAround = false;
        private bool isJumpClimbSuccess = false;

        [SerializeField] private float veloY;
        private void Start()
        {
            rigid = GetComponentInParent<Rigidbody2D>();
            ground = LayerMask.GetMask("Ground");
            anim = GetComponentInChildren<Animator>();
            horizontal = transform.localScale.x;

        }

        private void FixedUpdate()
        {

            if (!LevelManager.instance.isStarting)
            {
                return;
            }

            moveInput = Input.GetAxisRaw("Horizontal");

            if (moveInput > 0.1f)//左右镜像
            {
                transform.localScale = new Vector3(horizontal, transform.localScale.y, transform.localScale.z);
            }
            if (moveInput < -0.1f)
            {
                transform.localScale = new Vector3(-horizontal, transform.localScale.y, transform.localScale.z);
            }

            if (speed > maxSpeed * 0.75) //急转向
            {
                if (moveInput * recordMoveInput < 0) isTurnAround = true;
            }
            else
            {
                isTurnAround = false;
            }



            if (moveInput != 0)//加速
            {
                recordMoveInput = moveInput;
                if (speed < maxSpeed)
                {
                    if (isGround)
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
                        speed = maxSpeed;
                    }
                }

                rigid.velocity = new Vector2(moveInput * speed, rigid.velocity.y);
            }
            else//减速
            {

                if (speed > 0)
                {
                    if (isGround)
                    {
                        speed -= Time.fixedDeltaTime * decelerateInGround;
                    }
                    else //在空中
                    {
                        speed -= Time.fixedDeltaTime * decelerateInAir;
                    }

                }
                else
                {
                    speed = 0;
                    recordMoveInput = 0;
                }

                rigid.velocity = new Vector2(recordMoveInput * speed, rigid.velocity.y);
            }

            isGround = Physics2D.OverlapCircle(feetPos.position, feetCheckRadius, ground);
            isJumpClimbSuccess = Physics2D.OverlapCircle(jumpHandPos.position, handCheckRadius, ground);

            if (isJumpClimbSuccess && rigid.velocity.y < 0)//跳跃时用手够
            {
                if (Mathf.Abs(rigid.velocity.x) > handClimbSpeed.x || rigid.velocity.y < handClimbSpeed.y) //有横向速度或一定的下坠速度的时候
                {
                    transform.position = jumpHandPos.position + new Vector3(0, 1, 0);
                }
                else //垂直起跳
                {
                    if (Input.GetAxis("Vertical") > 0.5f)//按上
                    {
                        transform.position = new Vector3(transform.position.x, jumpHandPos.position.y + 1, 0);
                    }

                }

                isJumpClimbSuccess = false;
            }

            if (!isGround)
            {
                anim.SetBool("isJumpping", true);
            }
            else
            {
                if (anim.GetBool("isJumpping") && rigid.velocity.y == 0)
                {
                    anim.SetBool("isJumpping", false);
                    if (moveInput != 0)
                    {
                        anim.SetTrigger("JumpRunTrigger");
                    }
                }


                if (moveInput != 0)
                {
                    anim.SetBool("isRunning", true);
                }
                else
                {

                    anim.SetBool("isRunning", false);

                }
            }



        }

        private void Update()
        {
            if (!LevelManager.instance.isStarting)
            {
                return;
            }

            if (isGround && Input.GetKeyDown(KeyCode.Space))
            {
                rigid.velocity = Vector2.up * jumpForce;
                jumpTimer = jumpHeightTime;
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




            veloY = rigid.velocity.y;

        }


    }
}

