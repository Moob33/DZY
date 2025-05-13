using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("必填项：拖拽玩家对象到这里")]
    public Transform player;

    [Header("平滑时间（越小跟随越快）")]
    public float smoothTime = 0.3f;

    [Header("摄像机偏移（Z必须为负数！）")]
    public Vector3 offset = new Vector3(0, 0, -10);

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (player == null) return;

        // 计算目标位置
        Vector3 targetPosition = player.position + offset;

        // 平滑移动
        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref velocity,
            smoothTime
        );
    }
}
