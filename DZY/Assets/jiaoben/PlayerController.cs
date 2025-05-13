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
        HandleMovement();
        HandleAttack();

        // ��̬����FirePointλ�úͳ���
        if (firePoint != null)
        {
            //���ÿ��������ĵ���ҽ�ɫ����
            firePoint.position = transform.position;

            //��ȡ��ת����y���������жϳ���
            float direction = Mathf.Sign(transform.localScale.x); // ��ȡ������:-1, ��:1��
            firePoint.localRotation = Quaternion.Euler(0f,direction > 0 ? 0f : 180f,0f);

            //���¿�����λ��
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

        // ʹ��firePoint��λ�ú���תʵ�����ӵ�
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);
        Projectile projectileScript = projectile.GetComponent<Projectile>();

        if (projectileScript != null)
        {
            //ʹ��transform.right��ȡ����������3D��2D��
            Vector2 direction = transform.right;

            //������ת�Ƕ��жϣ�������2D��
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