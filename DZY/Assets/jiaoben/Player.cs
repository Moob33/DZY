using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public float qhsudu, zysudu;
    public GameObject bullet;
    private float verticalInput, horizontalInput;
    public float jumpForce;//����һ����Ծ����
    public float zlbs;//������������
    Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        Physics.gravity *= zlbs;
        rb = GetComponent<Rigidbody>();//��ȡ������������ϵĸ������
    }

    // Update is called once per frame
    void Update()
    {
        //verticalInput = Input.GetAxis("Vertical");//��ȡ��ֱ����������
        horizontalInput = Input.GetAxis("Horizontal");
        //transform.Translate(Vector3.forward * Time.deltaTime * qhsudu * verticalInput);
        transform.Translate(Vector3.right * Time.deltaTime * zysudu * horizontalInput);

        if (Input.GetKeyDown(KeyCode.F))//��ȡ���������ֵ�����¼���ĳ����
        {
            //���ɶ���Ԥ���壬λ�ã���ת�Ƕȣ�
            Instantiate(bullet, new Vector3(transform.position.x, bullet.transform.position.y, bullet.transform.position.z), bullet.transform.rotation);
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //rb.AddForce(0, jumpForce, 0,ForceMode.Impulse); Ч��ͬ����
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }
}
