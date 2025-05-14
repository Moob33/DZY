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
        //HandleMovement();
        HandleAttack();

        // 动态更新FirePoint位置和朝向
        if (firePoint != null)
        {
            // 保留旋转逻辑（方法四的核心修改是位置处理）
            float direction = Mathf.Sign(transform.localScale.x);
            firePoint.localRotation = Quaternion.Euler(0f, direction > 0 ? 0f : 180f, 0f);

            // === 方法四新增/修改的部分开始 ===
            // 获取当前本地位置（已在编辑器中设置好）
            Vector3 pos = firePoint.localPosition;

            // 保持X轴绝对值并应用朝向
            firePoint.localPosition = new Vector3(
                Mathf.Abs(pos.x) * direction, // X轴：绝对值*方向
                pos.y,                        // Y轴：保持编辑器设置的值
                pos.z                         // Z轴：保持不变
            );
            // === 方法四新增/修改的部分结束 ===
        }
    }

    private void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }

    private void HandleAttack()
    {
        if (Input.GetKeyDown(KeyCode.K) && Time.time >= lastAttackTime + attackCooldown)
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