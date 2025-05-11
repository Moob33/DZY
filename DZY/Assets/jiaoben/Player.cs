using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    //public float qhsudu, zysudu;
    public GameObject bullet;
    public float sudu;
    private float verticalInput, horizontalInput;
    public float jumpForce;//����һ����Ծ����
    public float zlbs;//������������
    private Animator anim;//�������
    private bool isMoving;//�ƶ�����ֵ
    Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity *= zlbs;
        rb = GetComponent<Rigidbody2D>();//��ȡ������������ϵĸ������
        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //verticalInput = Input.GetAxis("Vertical");//��ȡ��ֱ����������
        horizontalInput = Input.GetAxis("Horizontal");
        //transform.Translate(Vector3.forward * Time.deltaTime * qhsudu * verticalInput);
        //transform.Translate(Vector3.right * Time.deltaTime * zysudu * horizontalInput);
        rb.velocity = new Vector2(horizontalInput*sudu , rb.velocity.y);
       
        
        //��������
        if (Input.GetKeyDown(KeyCode.F))//��ȡ���������ֵ�����¼���ĳ����
        {
            //���ɶ���Ԥ���壬λ�ã���ת�Ƕȣ�
            //Instantiate(bullet, new Vector3(transform.position.x, bullet.transform.position.y, bullet.transform.position.z), bullet.transform.rotation);
            Instantiate(bullet, new Vector3(transform.position.x,transform.position.y,transform.position.z), bullet.transform.rotation);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //rb.AddForce(0, jumpForce, 0,ForceMode.Impulse); Ч��ͬ����
            rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
        }

        isMoving = rb.velocity.x != 0;
        anim.SetBool("isMoving", isMoving);
    }
}
