using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("�������ק��Ҷ�������")]
    public Transform player;

    [Header("ƽ��ʱ�䣨ԽС����Խ�죩")]
    public float smoothTime = 0.3f;

    [Header("�����ƫ�ƣ�Z����Ϊ��������")]
    public Vector3 offset = new Vector3(0, 0, -10);

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (player == null) return;

        // ����Ŀ��λ��
        Vector3 targetPosition = player.position + offset;

        // ƽ���ƶ�
        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref velocity,
            smoothTime
        );
    }
}
