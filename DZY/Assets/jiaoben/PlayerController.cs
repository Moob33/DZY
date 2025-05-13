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
        HandleMovement();
        HandleAttack();

        // 动态更新FirePoint位置和朝向
        if (firePoint != null)
        {
            //重置开火点的中心到玩家角色中心
            firePoint.position = transform.position;

            //获取旋转轴上y的数据来判断朝向
            float direction = Mathf.Sign(transform.localScale.x); // 获取朝向（左:-1, 右:1）
            firePoint.localRotation = Quaternion.Euler(0f,direction > 0 ? 0f : 180f,0f);

            //更新开火点的位置
            firePoint.localPosition = new Vector3(Mathf.Abs(firePoint.localPosition.x), firePoint.localPosition.y,firePoint.localPosition.z);
        }
    }

    private void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
    }

    private void HandleAttack()
    {
        if (Input.GetKeyDown(KeyCode.G) && Time.time >= lastAttackTime + attackCooldown)
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
            //使用transform.right获取朝向（适用于3D和2D）
            Vector2 direction = transform.right;

            //根据旋转角度判断（适用于2D）
            // float angle = transform.eulerAngles.y;
            // Vector2 direction = angle == 0 ? Vector2.right : Vector2.left;

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