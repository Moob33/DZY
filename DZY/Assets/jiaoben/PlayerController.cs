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
    private int mianxiang = 1;//�����ɫ������ͨ����ֵʵ��ö��
    private bool mianxiangYou = true;//Ĭ�������ұ�

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
        HandleAttack();
        Player player = GetComponent<Player>();
        bool isGround = player.isGround; // ��player�ű���ò��ֲ���

        // ��̬���� FirePoint ��λ�úͳ���
        if (firePoint != null)
        {
            // 1. ��ȡ��ɫ�ĵ�ǰ������/�ң�
            float direction = Mathf.Sign(transform.localScale.x);

            // 2. ���� FirePoint ����ת������ԭ�߼���
            firePoint.localRotation = Quaternion.Euler(0f, direction > 0 ? 0f : 180f, 0f);

            // 3. ���� FirePoint �ı������ƫ�����������õ� X �� Y ƫ�ƣ�
            Vector2 localOffset = new Vector2(0.5f, 0.2f); // ʾ��ֵ

            // 4. ��̬������������λ��
            firePoint.position = transform.position +
                                new Vector3(
                                    localOffset.x * direction, // X �᣺��������
                                    localOffset.y,            // Y �᣺�̶�ƫ��
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
            Debug.Log("����c��ֵ: " + isGround);
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

        // ʹ��firePoint��λ�ú���תʵ�����ӵ�
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