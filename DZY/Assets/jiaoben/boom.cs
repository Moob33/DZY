using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boom : MonoBehaviour
{
    [Header("爆炸设置")]
    [SerializeField] private GameObject explosionPrefab; // 拖入爆炸预制体
    [SerializeField] private float destroyDelay = 0.8f;  // 爆炸持续时间

    // 在挂载对象位置生成爆炸
    public void boom1()
    {
        if (explosionPrefab == null)
        {
            Debug.LogError("爆炸预制体未分配！", this);
            return;
        }

        // 在脚本挂载对象的位置生成爆炸
        GameObject explosion = Instantiate(
            explosionPrefab,
            transform.position, // 使用当前对象位置
            Quaternion.identity
        );

        Destroy(explosion, destroyDelay);
    }

}
