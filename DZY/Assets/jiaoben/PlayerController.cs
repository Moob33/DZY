using UnityEngine;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpForce = 10f;

    [Header("Combat")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float projectileDamage = 15f;
    [SerializeField] private DamageType projectileDamageType = DamageType.Physical;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float attackCooldown = 0.5f;

    private Rigidbody2D rb;
    private Health health;
    private float lastAttackTime = 0f;
    private int mianxiang = 1;//定义角色面向方向，通过数值实现枚举
    private bool mianxiangYou = true;//默认面向右边

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
        // 如果未指定firePoint，自动创建一个
        if (firePoint == null)
        {
            firePoint = new GameObject("FirePoint").transform;
            firePoint.SetParent(transform);
            firePoint.localPosition = new Vector3(0.5f, 0, 0); // 默认在玩家右侧
        }
    }

    private void Update()
    {
        HandleAttack();
        Player player = GetComponent<Player>();
        bool isGround = player.isGround; // 从player脚本获得部分参数

        // 动态更新 FirePoint 的位置和朝向
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

    private void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }

    private void HandleAttack()
    {
        Player player = GetComponent<Player>();
        if (player != null)
        {
            bool isGround = player.isGround;
            Debug.Log("变量c的值: " + isGround);
        }
        if (Input.GetKeyDown(KeyCode.K) && Time.time >= lastAttackTime + attackCooldown &&player!=null&&player.isGround)
        {
            ShootProjectile();
            lastAttackTime = Time.time;
        }
    }

    private void ShootProjectile()
    {
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


            projectileScript.Initialize(direction, projectileSpeed, projectileDamage, projectileDamageType, gameObject);
            Debug.Log("Projectile fired successfully in direction: " + direction);
        }
        else
        {
            Debug.LogError("Projectile script missing on projectile prefab");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyAttack"))
        {
            EnemyAttack enemyAttack = other.GetComponent<EnemyAttack>();
            if (enemyAttack != null)
            {
                DamageData damageData = new DamageData(
                    enemyAttack.damage,
                    enemyAttack.damageType,
                    other.gameObject
                );
                health.TakeDamage(damageData);
            }
        }
    }
    private void Filp()
    {
        mianxiang = mianxiang * -1;
        mianxiangYou = !mianxiangYou;
        transform.Rotate(0, 180, 0);
    }

    private void FilpControllor()
    {
        if (rb.velocity.x > 0 && !mianxiangYou)
        {
            Filp();
        }
        else if (rb.velocity.x < 0 && mianxiangYou)
        {
            Filp();
        }
    }
}