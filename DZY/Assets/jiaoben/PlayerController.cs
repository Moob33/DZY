using UnityEngine;

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

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();

        // ���δָ��firePoint���Զ�����һ��
        if (firePoint == null)
        {
            firePoint = new GameObject("FirePoint").transform;
            firePoint.SetParent(transform);
            firePoint.localPosition = new Vector3(0.5f, 0, 0); // Ĭ��������Ҳ�
        }
    }

    private void Update()
    {
        HandleMovement();
        HandleAttack();

        // ��̬����FirePointλ�úͳ���
        if (firePoint != null)
        {
            // ������������ĵ�����λ��
            firePoint.position = transform.position;

            // ������ҳ���תFirePoint
            float localX = Mathf.Sign(transform.localScale.x); // ��ȡ��ҵ�ǰ������:-1, ��:1��
            firePoint.localPosition = new Vector3(
                Mathf.Abs(firePoint.localPosition.x) * localX,
                firePoint.localPosition.y,
                firePoint.localPosition.z
            );
        }
    }

    private void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);

        // ��ת��ɫ����
        if (moveInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }

        if (Input.GetButtonDown("Jump") && Mathf.Abs(rb.velocity.y) < 0.001f)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
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

        Debug.Log("Attempting to shoot projectile"); // ��ӵ�����־

        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Projectile projectileScript = projectile.GetComponent<Projectile>();

        if (projectileScript != null)
        {
            Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
            projectileScript.Initialize(direction, projectileSpeed, projectileDamage, projectileDamageType, gameObject);
            Debug.Log("Projectile fired successfully"); // ��ӳɹ�������־
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
}