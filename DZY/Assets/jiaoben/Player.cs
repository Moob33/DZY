using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    //public float qhsudu, zysudu;
    public GameObject bullet;
    public float sudu;
    private float horizontalInput;//��ȡˮƽ����
    public float jumpForce;//����һ����Ծ����
    public float zlbs;//������������
    private Animator anim;//�������
    private bool isMoving;//�ƶ�����ֵ
    private int mianxiang=1;//�����ɫ������ͨ����ֵʵ��ö��
    private bool mianxiangYou = true;//Ĭ�������ұ�
    public bool isGround = false;
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
        horizontalInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(horizontalInput*sudu , rb.velocity.y);//��������λ�ñ����������ٶȱ���Ա���״̬���л�
       

        //�������� zqh
        if (Input.GetKeyDown(KeyCode.Space) && isGround == true)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
            isGround = false;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            isGround = true;
        }


        //�����ٶ��жϽ�ɫ�Ƿ����ƶ�״̬���������ֵ  zqh
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
