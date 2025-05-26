using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boom : MonoBehaviour
{
    [Header("��ը����")]
    [SerializeField] private GameObject explosionPrefab; // ���뱬ըԤ����
    [SerializeField] private float destroyDelay = 0.8f;  // ��ը����ʱ��

    // �ڹ��ض���λ�����ɱ�ը
    public void boom1()
    {
        if (explosionPrefab == null)
        {
            Debug.LogError("��ըԤ����δ���䣡", this);
            return;
        }

        // �ڽű����ض����λ�����ɱ�ը
        GameObject explosion = Instantiate(
            explosionPrefab,
            transform.position, // ʹ�õ�ǰ����λ��
            Quaternion.identity
        );

        Destroy(explosion, destroyDelay);
    }

}
