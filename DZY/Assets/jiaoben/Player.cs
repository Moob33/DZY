using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    //public float qhsudu, zysudu;
    public GameObject bullet;
    public float sudu;
    private float horizontalInput;//获取水平输入
    public float jumpForce;//定义一个跳跃的力
    public float zlbs;//定义重力倍数
    private Animator anim;//动画组件
    private bool isMoving;//移动布尔值
    private int mianxiang=1;//定义角色面向方向，通过数值实现枚举
    private bool mianxiangYou = true;//默认面向右边
    public bool isGround = false;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity *= zlbs;
        rb = GetComponent<Rigidbody2D>();//获取挂载在玩家身上的刚体组件
        anim = GetComponentInChildren<Animator>();//获取动画组件

    }

    // Update is called once per frame
    void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(horizontalInput*sudu , rb.velocity.y);//舍弃上述位置变更代码改用速度变更以便于状态的切换
       

        //键盘输入 zqh
        if (Input.GetKeyDown(KeyCode.Space) && isGround == true)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
            isGround = false;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            isGround = true;
        }


        //根据速度判断角色是否处于移动状态，变更布尔值  zqh
        isMoving = rb.velocity.x != 0;

        anim.SetFloat("Y velocity", rb.velocity.y);
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isGround", isGround);

        
        FilpControllor();

    }

    private void Filp()
    {
        mianxiang = mianxiang * -1;
        mianxiangYou = !mianxiangYou;
        transform.Rotate(0, 180, 0);
    }

    private void FilpControllor()
    {
        if (rb.velocity.x > 0&&!mianxiangYou)
        {
            Filp();
        }
        else if(rb.velocity.x < 0 && mianxiangYou)
        {
            Filp();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("db"))
        {
            isGround = true;
        }
    }
    }
