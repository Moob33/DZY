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
        // ��Ҫ�˺�������
        if (other.gameObject == owner) return;

        // ֻ�˺�����
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
        // ����ǽ�ڵ��ϰ���Ҳ����
        else if (other.CompareTag("db"))
        {
            Destroy(gameObject);
        }
    }
}