using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float damage;
    public DamageType damageType;

    public void SetDamage(float amount, DamageType type)
    {
        damage = amount;
        damageType = type;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Health playerHealth = other.GetComponent<Health>();
            if (playerHealth != null)
            {
                DamageData damageData = new DamageData(damage, damageType, gameObject);
                playerHealth.TakeDamage(damageData);
            }
        }
    }
}