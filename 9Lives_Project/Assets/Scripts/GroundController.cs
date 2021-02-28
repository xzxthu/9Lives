using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundController : MonoBehaviour
{
    [HideInInspector] public bool isSetted = false;
    [HideInInspector] public int thisSettingNum;

    //[HideInInspector] public static GroundController instance;
    private Animator anim;



    void Start()
    {
        CloseGround();
        anim = GetComponent<Animator>();


        if(!LevelManager.instance.isDrawMode) //读取模式
        {
            RestoreGround();
            this.isSetted = true;
            anim.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(LevelManager.instance.isStarting) //过了片头
        {
            if (!isSetted)//未放置
            {
                transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0, 0, 10));//鼠标跟随

                if (Input.GetMouseButtonDown(0))
                {
                    Instantiate(this.gameObject);
                    RestoreGround();
                    this.isSetted = true;
                    this.anim.SetBool("isSetted", true);
                }
            }
            else//放置后
            {
                DestroyGround();

                if (Input.GetAxis("Vertical") < -0.1f)//One Way Down
                {
                    CloseGround();
                    Invoke("RestoreGround", LevelManager.instance.oneWayRestoreTime);
                }
            }
        }
        

        
    }

    private void CloseGround()//One way down 使用
    {
        GetComponent<BoxCollider2D>().enabled = false;
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    public void RestoreGround() //One way down 恢复使用
    {
        this.gameObject.layer = LayerMask.NameToLayer("Ground");
        GetComponent<BoxCollider2D>().enabled = true;
    }

    public void SetTheGround()
    {

        RestoreGround();
        this.isSetted = true;
        this.anim.SetBool("isSetted", true);
    }

    public void DestroyGround()//右键取消
    {
        if(Input.GetMouseButtonDown(1))
        {

            //Debug.Log("Right!");
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if(
                (mousePos.x >(transform.position.x-1.5f)) && (mousePos.x < (transform.position.x + 1.5f)) &&
                (mousePos.y > (transform.position.y - 0.25f)) && (mousePos.y < (transform.position.y + 0.25f))
                )
            {

                Destroy(this.gameObject);

                //Debug.Log("Into!");
            }
            
        }
        
    }
}
