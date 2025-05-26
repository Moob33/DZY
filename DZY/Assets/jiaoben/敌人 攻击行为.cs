using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class 敌人攻击行为 : MonoBehaviour
{
    [Header("攻击模式")]
    [SerializeField] private bool 近战;
    [SerializeField] private bool 远程;

    [Header("近战参数")]
    [SerializeField] private float 近战攻击间隔 = 3f;
    [SerializeField] private float 近战攻击持续时间 = 0.5f;
    public GameObject 挥爪判定;

    [Header("远程参数")]
    [SerializeField] private float 远程攻击间隔 = 3f;
    [SerializeField] private float 远程攻击持续时间 = 0.5f;
    [SerializeField] private GameObject 子弹预制体;
    [SerializeField] private Transform 发射点;
    [SerializeField] private float 子弹速度 = 10f;
    public GameObject ccpd;
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject projectilePrefab;

    private Animator anim;
    private float 近战下次攻击时间;
    private bool 正在近战;
    private float 远程下次攻击时间;
    private bool 正在远程;

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        近战下次攻击时间 = Time.time + 近战攻击间隔;
        远程下次攻击时间 = Time.time + 远程攻击间隔;

        // 自动查找发射点（如果没有手动指定）
        if (firePoint == null)
        {
            firePoint = new GameObject("FirePoint").transform;
            firePoint.SetParent(transform);
            firePoint.localPosition = new Vector3(0.5f, 0, 0); // 默认在玩家右侧
        }
    }


    void Update()
    {
        if (近战) 可以近战();
        if (远程) 可以远程();

        if (正在近战 && Time.time >= 近战下次攻击时间 - 近战攻击间隔 + 近战攻击持续时间)
        {
            结束近战攻击();
        }
        if (正在远程 && Time.time >= 远程下次攻击时间 - 远程攻击间隔 + 远程攻击持续时间)
        {
            结束远程攻击();
        }
        if (firePoint != null)
        {
            // 1. 获取角色的当前朝向（左/右）
            float direction = Mathf.Sign(transform.localScale.x);

            // 2. 设置 FirePoint 的旋转（保持原逻辑）
            firePoint.localRotation = Quaternion.Euler(0f, direction > 0 ? 0f : 180f, 0f);

            // 3. 定义 FirePoint 的本地相对偏移量（可配置的 X 和 Y 偏移）
            Vector2 localOffset = new Vector2(0.5f, 0.2f); // 示例值

            // 4. 动态计算世界坐标位置
            firePoint.position = transform.position +
                                new Vector3(
                                    localOffset.x * direction, // X 轴：方向敏感
                                    localOffset.y,            // Y 轴：固定偏移
                                    0f
                                );
        }
    }

    private void 可以远程()
    {
        if (Time.time >= 远程下次攻击时间 && !正在远程)
        {
            开始远程攻击();
        }
    }

    private void 可以近战()
    {
        if (Time.time >= 近战下次攻击时间 && !正在近战)
        {
            开始近战攻击();
        }
    }

    void 开始近战攻击()
    {
        正在近战 = true;
        anim.SetBool("enemy attack", true);
        anim.SetTrigger("gjz");
        Debug.Log("敌人开始攻击！时间: " + Time.time);

        // 设置下次攻击时间
        近战下次攻击时间 = Time.time + 近战攻击间隔;
    }

    void 结束近战攻击()
    {
        正在近战= false;
        anim.SetBool("enemy attack", false);
        Debug.Log("敌人攻击结束");
    }
    void 开始远程攻击()
    {
        正在远程 = true;
        anim.SetBool("enemy shoot", true);
        anim.SetTrigger("cc");

        // 发射子弹
        if (projectilePrefab == null || firePoint == null)
        {
            Debug.LogError("Projectile prefab or fire point not set!");
            return;
        }

        Debug.Log("Attempting to shoot projectile");

        // 使用firePoint的位置和旋转实例化子弹
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Projectile projectileScript = projectile.GetComponent<Projectile>();

        if (projectileScript != null)
        {
            Vector2 direction = transform.right;

            Debug.Log("Projectile fired successfully in direction: " + direction);
        }
        else
        {
            Debug.LogError("Projectile script missing on projectile prefab");
        }

        远程下次攻击时间 = Time.time + 远程攻击间隔;
    }

    void 结束远程攻击()
    {
        正在远程 = false;
        anim.SetBool("enemy shoot", false);
        Debug.Log("敌人攻击结束");
    }

    private void gjz()
    {
        挥爪判定.SetActive(true);
    }
    private void gjz2()
    {
        挥爪判定.SetActive(false);
    }

    private void cc()
    {
        ccpd.SetActive(true);
    }

    private void ccjs()
    {
        ccpd.SetActive(false);
    }
}
