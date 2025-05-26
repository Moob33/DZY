using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private bool isPlayer = false;

    [Header("Defense Attributes")]
    [SerializeField] private float physicalDefense = 0f;
    [SerializeField] private float energyDefense = 0f;

    [Header("受击效果")]
    [SerializeField] private float hitEffectDuration = 0.3f;
    private Coroutine hitCoroutine;

    private float currentHealth;
    private bool isInvulnerable = false;
    public bool over,isHit;
    private Animator anim1;
    private AudioSource hit;
    // 事件，用于通知其他组件血量变化
    public delegate void HealthChanged(float current, float max);
    public event HealthChanged OnHealthChanged;

    public delegate void Death(GameObject who);
    public event Death OnDeath;

    private void Awake()
    {
        currentHealth = maxHealth;
        anim1 = GetComponentInChildren<Animator>();
        hit = GetComponent<AudioSource>();
    }

    public void TakeDamage(DamageData damageData)
    {
        if (isInvulnerable || currentHealth <= 0) return;

        // 计算伤害倍率基于防御属性
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

        // 触发血量变化事件
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
        hitCoroutine = StartCoroutine(TriggerHitEffect());

        // 调试日志
        Debug.Log($"{gameObject.name} took {finalDamage} {damageData.damageType} damage (Multiplier: {damageMultiplier})");

        if (currentHealth <= 0)
        {
           // over = true;
            //anim1.SetBool("over", over);
            Die();
        }
        Debug.Log($"即将触发血量更新事件，当前血量：{currentHealth}");
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
            GetComponent<boom>().boom1();
            Destroy(gameObject);

        }
        else
        {
            // 玩家死亡逻辑，比如游戏结束
            over = true;
            anim1.SetBool("over", over );
            StartCoroutine(LoadNextSceneAfterDelay(2f));
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            Debug.Log("Game Over!");
        }
    }
    private IEnumerator LoadNextSceneAfterDelay(float delaySeconds)
    {
        Debug.Log($"等待 {delaySeconds} 秒后跳转场景...");
        yield return new WaitForSeconds(delaySeconds);

        // 场景跳转逻辑
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
            Debug.Log("已跳转到下一场景");
        }
    }
    private IEnumerator TriggerHitEffect()
    {
        // 取消正在进行的受击效果
        if (hitCoroutine != null)
        {
            StopCoroutine(hitCoroutine);
        }

        // 设置受击状态
        isHit = true;
        anim1.SetBool("isHit", isHit);

        // 等待受击持续时间
        yield return new WaitForSeconds(hitEffectDuration);

        // 重置受击状态
        isHit = false;
        anim1.SetBool("isHit", isHit);

        hitCoroutine = null;
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

// 伤害类型枚举
public enum DamageType
{
    Physical,
    Energy
}

// 伤害数据结构
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