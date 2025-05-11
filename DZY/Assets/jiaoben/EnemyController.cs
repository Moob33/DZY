using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float patrolDistance = 3f;

    [Header("Combat")]
    [SerializeField] private float attackDamage = 5f;
    [SerializeField] private DamageType attackDamageType = DamageType.Physical;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private float attackCooldown = 1f;
    [SerializeField] private GameObject attackHitboxPrefab;
    [SerializeField] private float attackHitboxDuration = 0.2f;

    private Rigidbody2D rb;
    private Health health;
    private float lastAttackTime = 0f;
    private Vector2 startPosition;
    private bool movingRight = true;
    private Transform playerTransform;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health>();
        startPosition = transform.position;

        // 查找玩家
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player not found in scene!");
        }
    }

    private void Update()
    {
        if (playerTransform == null) return;

        Patrol();

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= attackRange)
        {
            if (Time.time >= lastAttackTime + attackCooldown)
            {
                Attack();
                lastAttackTime = Time.time;
            }
        }
    }

    private void Patrol()
    {
        if (movingRight)
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
            if (transform.position.x > startPosition.x + patrolDistance)
            {
                movingRight = false;
                FlipSprite();
            }
        }
        else
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
            if (transform.position.x < startPosition.x - patrolDistance)
            {
                movingRight = true;
                FlipSprite();
            }
        }
    }

    private void FlipSprite()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void Attack()
    {
        if (attackHitboxPrefab == null)
        {
            Debug.LogError("Attack hitbox prefab is not assigned!");
            return;
        }

        // 创建攻击判定框
        Vector2 attackPosition = (Vector2)transform.position +
                               (movingRight ? Vector2.right : Vector2.left) * attackRange;

        GameObject hitbox = Instantiate(attackHitboxPrefab, attackPosition, Quaternion.identity);
        EnemyAttack attack = hitbox.GetComponent<EnemyAttack>();

        if (attack == null)
        {
            Debug.LogError("EnemyAttack component missing on hitbox prefab");
            Destroy(hitbox);
            return;
        }

        attack.SetDamage(attackDamage, attackDamageType);
        Destroy(hitbox, attackHitboxDuration);

        Debug.Log("Enemy attacked!");
    }

    // 可视化攻击范围（仅在编辑器中显示）
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}