using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    //冲刺相关设置
    [Header("dash")]
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashTime;//冲刺过程计时器
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashCD;
    [SerializeField] private float dashCDTime;//冲刺CD计时器

    [Header("attack")]
    [SerializeField] private float attackDuration;
    [SerializeField] private float attackTime;//近战过程计时器
    [SerializeField] private float attackCD;
    [SerializeField] private float attackCDTime;//近战CD计时器

    [Header("attack")]
    [SerializeField] private float attack2Duration;
    [SerializeField] private float attack2Time;//远程过程计时器
    [SerializeField] private float attack2CD;
    [SerializeField] private float attack2CDTime;//远程CD计时器

    [Header("move")]
    public float sudu;
    private float horizontalInput;//获取水平输入
    public float jumpForce;//定义一个跳跃的力
    public float zlbs;//定义重力倍数
    private Animator anim;//动画组件
    private bool isMoving;//移动布尔值
    private int mianxiang=1;//定义角色面向方向，通过数值实现枚举
    private bool mianxiangYou = true;//默认面向右边
    public bool isGround = false;
    private bool isAttack;
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
        Move();
        Input();

        dashTime = dashTime - Time.deltaTime;
        dashCDTime = dashCDTime - Time.deltaTime;

        attackTime = attackTime - Time.deltaTime;
        attackCDTime = attackCDTime - Time.deltaTime;

        attack2Time = attack2Time - Time.deltaTime;
        attack2CDTime = attack2CDTime - Time.deltaTime;

        AnimatorControllers();
        FilpControllor();

    }


    private void Input()
    {
        horizontalInput = UnityEngine.Input.GetAxis("Horizontal");


        //键盘输入 zqh
        if (UnityEngine.Input.GetKeyDown(KeyCode.Space) && isGround == true)
        {
            Jump();
            isGround = false;
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.L))
        {
            Dash();
        }


        if (UnityEngine.Input.GetKeyDown(KeyCode.J))
        {
            Attack();
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.K))
        {
            Attack2();
        }

        if (UnityEngine.Input.GetKeyDown(KeyCode.R))
        {
            isGround = true;
        }
    }

    private void Dash()
    {
        if (dashCDTime < 0)
        {
            dashTime = dashDuration;
            dashCDTime = dashCD;
        }
    }

    private void Attack()
    {
        if (attackCDTime < 0)
        {
            attackTime = attackDuration;
            attackCDTime = attackCD;
        }
    }
    private void Attack2()
    {
        if (attack2CDTime < 0)
        {
            attack2Time = attack2Duration;
            attack2CDTime = attack2CD;
        }
    }

    private void Move()
    {
        if (dashTime > 0)
        {
            //rb.velocity = new Vector2(horizontalInput * dashSpeed, rb.velocity.y);
            rb.velocity = new Vector2(horizontalInput * dashSpeed, 0);//y轴速度改为0使玩家不会失去冲刺时的y轴变量（空中不会下落）
        }
        else
        {
            rb.velocity = new Vector2(horizontalInput * sudu, rb.velocity.y);//舍弃上述位置变更代码改用速度变更以便于状态的切换
        }
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
    }

    private void AnimatorControllers()
    {
        //根据速度判断角色是否处于移动状态，变更布尔值  zqh
        isMoving = rb.velocity.x != 0;

        anim.SetFloat("Y velocity", rb.velocity.y);
        anim.SetBool("isMoving", isMoving);
        anim.SetBool("isGround", isGround);
        anim.SetBool("isDashing", dashTime > 0);
        anim.SetBool("isAttack", attackTime>0);
        anim.SetBool("isShoot", attack2Time > 0);

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
