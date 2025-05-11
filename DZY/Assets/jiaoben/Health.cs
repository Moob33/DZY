using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private bool isPlayer = false;

    [Header("Defense Attributes")]
    [SerializeField] private float physicalDefense = 0f;
    [SerializeField] private float energyDefense = 0f;

    private float currentHealth;
    private bool isInvulnerable = false;

    // �¼�������֪ͨ�������Ѫ���仯
    public delegate void HealthChanged(float current, float max);
    public event HealthChanged OnHealthChanged;

    public delegate void Death(GameObject who);
    public event Death OnDeath;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(DamageData damageData)
    {
        if (isInvulnerable || currentHealth <= 0) return;

        // �����˺����ʻ��ڷ�������
        float damageMultiplier = 1f;

        if (damageData.damageType == DamageType.Physical)
        {
            damageMultiplier = 1f - (physicalDefense / (physicalDefense + 100f));
        }
        else if (damageData.damageType == DamageType.Energy)
        {
            damageMultiplier = 1f - (energyDefense / (energyDefense + 100f));
        }

        float finalDamage = damageData.damageAmount * damageMultiplier;
        currentHealth -= finalDamage;

        // ����Ѫ���仯�¼�
        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        // ������־
        Debug.Log($"{gameObject.name} took {finalDamage} {damageData.damageType} damage (Multiplier: {damageMultiplier})");

        if (currentHealth <= 0)
        {
            Die();
        }
        Debug.Log($"��������Ѫ�������¼�����ǰѪ����{currentHealth}");
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} died!");
        OnDeath?.Invoke(gameObject);

        if (!isPlayer)
        {
            Destroy(gameObject);
        }
        else
        {
            // ��������߼���������Ϸ����
            Debug.Log("Game Over!");
        }
    }

    public void SetInvulnerable(bool invulnerable)
    {
        isInvulnerable = invulnerable;
    }

    public float GetCurrentHealth()
    {
        return currentHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }
}

// �˺�����ö��
public enum DamageType
{
    Physical,
    Energy
}

// �˺����ݽṹ
public struct DamageData
{
    public float damageAmount;
    public DamageType damageType;
    public GameObject damageSource;

    public DamageData(float amount, DamageType type, GameObject source)
    {
        damageAmount = amount;
        damageType = type;
        damageSource = source;
    }
}