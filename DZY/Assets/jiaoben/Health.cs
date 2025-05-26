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

    [Header("�ܻ�Ч��")]
    [SerializeField] private float hitEffectDuration = 0.3f;
    private Coroutine hitCoroutine;

    private float currentHealth;
    private bool isInvulnerable = false;
    public bool over,isHit;
    private Animator anim1;
    private AudioSource hit;
    // �¼�������֪ͨ�������Ѫ���仯
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
        hitCoroutine = StartCoroutine(TriggerHitEffect());

        // ������־
        Debug.Log($"{gameObject.name} took {finalDamage} {damageData.damageType} damage (Multiplier: {damageMultiplier})");

        if (currentHealth <= 0)
        {
           // over = true;
            //anim1.SetBool("over", over);
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
            GetComponent<boom>().boom1();
            Destroy(gameObject);

        }
        else
        {
            // ��������߼���������Ϸ����
            over = true;
            anim1.SetBool("over", over );
            StartCoroutine(LoadNextSceneAfterDelay(2f));
            //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            Debug.Log("Game Over!");
        }
    }
    private IEnumerator LoadNextSceneAfterDelay(float delaySeconds)
    {
        Debug.Log($"�ȴ� {delaySeconds} �����ת����...");
        yield return new WaitForSeconds(delaySeconds);

        // ������ת�߼�
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
            Debug.Log("����ת����һ����");
        }
    }
    private IEnumerator TriggerHitEffect()
    {
        // ȡ�����ڽ��е��ܻ�Ч��
        if (hitCoroutine != null)
        {
            StopCoroutine(hitCoroutine);
        }

        // �����ܻ�״̬
        isHit = true;
        anim1.SetBool("isHit", isHit);

        // �ȴ��ܻ�����ʱ��
        yield return new WaitForSeconds(hitEffectDuration);

        // �����ܻ�״̬
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