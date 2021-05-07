using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerActor : Actor
{
    // pamameters for move and animation control
    public float maxWalkSpeed = 0.5f;                           //最大速度 max speed of walking
    public float maxRunSpeed = 1f;                              //最大速度 max spped of running
    public float accelerate = 1f;                               //加速 acclecrate of running
    public float decelerateOnGround = 1f;                       //在地面上才横向减速（空中不减速） decelerate on ground
    public float decelerateInAir = 1f;                          //空中减速 decelerate in air
    public float decelerateWhenSlipping = 1f;                   //打滑减速 decelerate when slips
    public float slippingSpeed = 7f;
    public float changeDirectionDecelerate = 2f;                //转向会横向减速 decelerate when turn
    public float speedForHighLandding = -15;                    //大落地动作的下坠速度 velocity for play high-landing animation
    public float jumpForce;
    public float jumpHeightTime;
    public LayerMask ground;
    public LayerMask banana;

    [Header("Cat's Face")]
    public CatFace catFace;

    [Header("Cat's Effect")]
    public CharacterEffect catEffect;

    // references and pamameters for feet's methods
    [Header("Cat's Feet")]
    public Transform feetPos;
    public float feetCheckRadius;
    public Transform Left_Leg;
    public Transform Right_Leg;
    public float handCheckRadius;
    public Transform Left_BackLeg;
    public Transform Right_BackLeg;

    [Header("Hurted Pamameters")]
    public float hurtForce = 4f;
    public float hurtTime = 0.5f;
    [HideInInspector] public Vector2 hurtDir = Vector2.left;

    #region private pamameters
    // private pamameters
    [HideInInspector] public float moveInput;
     public float recordMoveInput;             
    public float speed = 0f;
    [HideInInspector] public float jumpTimer;
    [HideInInspector] public float acTimer;
    [HideInInspector] public float horizontal;


    [HideInInspector] public bool isGround;
    [HideInInspector] public bool isLeftHandCatch = false;
    [HideInInspector] public bool isRightHandCatch = false;
    [HideInInspector] public bool isTurnAround = false;
    [HideInInspector] public bool isJumpClimbSuccess = false;
    [HideInInspector] public bool isJumpHoriz = false;
    [HideInInspector] public bool isPressSpace = false;
    [HideInInspector] public bool needIK = false;
    [HideInInspector] public bool isHurted;
    [HideInInspector] public bool isHanging;
    [HideInInspector] public bool isSlipping;
    [HideInInspector] public bool isDown; // Character level from a platform by one way
    private bool needCheckAutoDown;
    [HideInInspector] public bool jumpToRun;

    // references of components
    [HideInInspector] public Animator anim;
    [HideInInspector] public Transform catTransform;
    [HideInInspector] public Rigidbody2D rigid;
    [HideInInspector] public CapsuleCollider2D colli;
    [HideInInspector] public Skin skin;

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
        if(catFace == null)
        {
            catFace = GetComponentInChildren<CatFace>();
            Debug.Log("No Player Face Controller");
        }
        if(catEffect == null)
        {
            catEffect = GetComponentInChildren<CharacterEffect>();
            Debug.Log("No Player Effect Controller");
        }


        rigid = GetComponent<Rigidbody2D>();
        colli = GetComponent<CapsuleCollider2D>();
        horizontal = transform.localScale.x;
        skin = GetComponentInChildren<Skin>();

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
        _actorStateDic[ActorStateType.Hurted] = new HurtedState();
        _actorStateDic[ActorStateType.Slip] = new SlipState();
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
        isSlipping = (Physics2D.OverlapCircle(feetPos.position, handCheckRadius, banana)||
            Physics2D.OverlapCircle(Left_Leg.position, handCheckRadius, banana) ||
            Physics2D.OverlapCircle(Right_Leg.position, handCheckRadius, banana));
    }

    private void MirrorDirection()
    {
        if (moveInput > 0.1f)//左右镜像
        {
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
        if (moveInput < -0.1f)
        {
            transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
        }
    }

    private void CalculateFeetPoint()
    {
        float direction = transform.localScale.x > 0 ? -1 : 1;
        float updateFeetX = (Left_BackLeg.position.x + Right_BackLeg.position.x) * 0.5f;
        feetPos.position = new Vector3(updateFeetX, feetPos.position.y, feetPos.position.z); //根据后脚位置更新
        if(isGround&&moveInput!=0)
        {
            colli.offset = new Vector2(-2f,1);

            updateFeetX = transform.position.x + direction * 1.5f; //野狼
            feetPos.position = new Vector3(updateFeetX, feetPos.position.y, feetPos.position.z); //根据后脚位置更新

            return;
        }

        colli.offset = new Vector2(0, 1);
    }


    public void CheckAutoDown()
    {
        if (!isLeftHandCatch && !isRightHandCatch && isGround && rigid.velocity.y == 0 && moveInput==0 &&
            !CheckJumpAnimation()) //平的时候手出地面范围了，无输入的时候不判定,垂直起跳降落不判定
        {
            if(!needCheckAutoDown)
                StartCoroutine(LateCheckAutoDown());
            
        }
    }

    private IEnumerator LateCheckAutoDown()
    {
        needCheckAutoDown = true;

        yield return new WaitForSeconds(0.5f);

        if(!isLeftHandCatch && !isRightHandCatch && isGround && rigid.velocity.y == 0 && 
            moveInput == 0 &&!CheckJumpAnimation()) //再次检测
        {
            float offsetX = (transform.position.x - Left_Leg.position.x) *
                (rigid.velocity.x == 0f ? 0f : 1f); //有输入时检测移到后脚
            Vector3 offset = new Vector3(offsetX, 0.5f, 0);
            //Debug.Log(offset.x);

            //offset = Vector3.zero;
            Ray2D rayUpLeft = new Ray2D(Left_Leg.position + offset, Vector2.down);
            Ray2D rayUpRight = new Ray2D(Right_Leg.position + offset, Vector2.down);

            RaycastHit2D infoLeft = Physics2D.Raycast(rayUpLeft.origin, Vector2.down, 1.5f, ground);
            RaycastHit2D infoRight = Physics2D.Raycast(rayUpRight.origin, Vector2.down, 1.5f, ground);

            //Debug.DrawRay(rayUpLeft.origin, Vector2.down, Color.yellow);
            //Debug.DrawRay(rayUpRight.origin, Vector2.down, Color.yellow);

            if (infoLeft.collider == null && infoRight.collider == null)
            {
                Debug.Log("检测到手超出范围，自动下落");
                PlayerActor.instance.isDown = true;
            }
        }

        needCheckAutoDown = false;
    }

    private bool CheckJumpAnimation() //检测是否现在是垂直起跳的姿态
    {
        try
        {
            string animString = anim.GetCurrentAnimatorClipInfo(0)[0].clip.name;
            if (animString == "Test_2_Cat@JumingUp") return true;
        }
        catch
        { }
        
        //Debug.Log(animString);
        

        return false;
    }
}
