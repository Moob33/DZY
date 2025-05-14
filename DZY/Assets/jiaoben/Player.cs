using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    //����������
    [Header("dash")]
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashTime;//��̹��̼�ʱ��
    [SerializeField] private float dashSpeed;
    [SerializeField] private float dashCD;
    [SerializeField] private float dashCDTime;//���CD��ʱ��

    [Header("attack")]
    [SerializeField] private float attackDuration;
    [SerializeField] private float attackTime;//��ս���̼�ʱ��
    [SerializeField] private float attackCD;
    [SerializeField] private float attackCDTime;//��սCD��ʱ��

    [Header("attack")]
    [SerializeField] private float attack2Duration;
    [SerializeField] private float attack2Time;//Զ�̹��̼�ʱ��
    [SerializeField] private float attack2CD;
    [SerializeField] private float attack2CDTime;//Զ��CD��ʱ��

    [Header("move")]
    public float sudu;
    private float horizontalInput;//��ȡˮƽ����
    public float jumpForce;//����һ����Ծ����
    public float zlbs;//������������
    private Animator anim;//�������
    private bool isMoving;//�ƶ�����ֵ
    private int mianxiang=1;//�����ɫ������ͨ����ֵʵ��ö��
    private bool mianxiangYou = true;//Ĭ�������ұ�
    public bool isGround = false;
    private bool isAttack;
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity *= zlbs;
        rb = GetComponent<Rigidbody2D>();//��ȡ������������ϵĸ������
        anim = GetComponentInChildren<Animator>();//��ȡ�������

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


        //�������� zqh
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
            rb.velocity = new Vector2(horizontalInput * dashSpeed, 0);//y���ٶȸ�Ϊ0ʹ��Ҳ���ʧȥ���ʱ��y����������в������䣩
        }
        else
        {
            rb.velocity = new Vector2(horizontalInput * sudu, rb.velocity.y);//��������λ�ñ����������ٶȱ���Ա���״̬���л�
        }
    }

    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
    }

    private void AnimatorControllers()
    {
        //�����ٶ��жϽ�ɫ�Ƿ����ƶ�״̬���������ֵ  zqh
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
