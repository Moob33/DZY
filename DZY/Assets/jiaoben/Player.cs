using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    //public float qhsudu, zysudu;
    public GameObject bullet;
    public float sudu;
    private float verticalInput, horizontalInput;
    public float jumpForce;//定义一个跳跃的力
    public float zlbs;//定义重力倍数
    private Animator anim;//动画组件
    private bool isMoving, zdbs;//移动布尔值
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity *= zlbs;
        zdbs = true;
        rb = GetComponent<Rigidbody2D>();//获取挂载在玩家身上的刚体组件
        anim = GetComponentInChildren<Animator>();//获取动画组件
    }

    // Update is called once per frame
    void Update()
    {
        //verticalInput = Input.GetAxis("Vertical");//获取垂直轴输入数据
        horizontalInput = Input.GetAxis("Horizontal");
        //transform.Translate(Vector3.forward * Time.deltaTime * qhsudu * verticalInput);
        //transform.Translate(Vector3.right * Time.deltaTime * zysudu * horizontalInput);
        rb.velocity = new Vector2(horizontalInput*sudu , rb.velocity.y);//舍弃上述位置变更代码改用速度变更以便于状态机的切换
       
        
        //键盘输入 zqh
        if (Input.GetKeyDown(KeyCode.F))//获取键盘输入的值，按下键盘某个键
        {
            //生成对象（预制体，位置，旋转角度）
            //Instantiate(bullet, new Vector3(transform.position.x, bullet.transform.position.y, bullet.transform.position.z), bullet.transform.rotation);
            Instantiate(bullet, new Vector3(transform.position.x,transform.position.y,transform.position.z), bullet.transform.rotation);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //rb.AddForce(0, jumpForce, 0,ForceMode.Impulse); 效果同下行
            rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
            zdbs = false;
        }

        //根据速度判断角色是否处于移动状态，变更布尔值  zqh
        isMoving = rb.velocity.x != 0;
        anim.SetBool("isMoving", isMoving);
        
    }
}
