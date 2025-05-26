using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifetime = 2f;

    private Rigidbody2D rb;
    private float damage;
    private DamageType damageType;
    private GameObject owner;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Destroy(gameObject, lifetime);
    }

    public void Initialize(Vector2 direction, float speed, float damageAmount, DamageType type, GameObject ownerObject)
    {
        rb.velocity = direction * speed;
        damage = damageAmount;
        damageType = type;
        owner = ownerObject;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 不要伤害发射者
        if (other.gameObject == owner) return;

        // 只伤害敌人
        if (other.CompareTag("Enemy"))
        {
            Health enemyHealth = other.GetComponent<Health>();
            if (enemyHealth != null)
            {
                DamageData damageData = new DamageData(damage, damageType, gameObject);
                enemyHealth.TakeDamage(damageData);
            }

            Destroy(gameObject);
        }
        // 碰到墙壁等障碍物也销毁
        else if (other.CompareTag("db"))
        {
            Destroy(gameObject);
        }
    }
}