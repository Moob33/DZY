using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthUI : MonoBehaviour
{
    [Header("Player Reference")]
    [Tooltip("��קPlayer��������")]
    [SerializeField] private Health playerHealth;  // �༭����ק�������

    [Header("UI References")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;

    [Header("Style Settings")]
    [SerializeField] private Color healthyColor = Color.green;
    [SerializeField] private Color cautionColor = Color.yellow;
    [SerializeField] private Color dangerColor = Color.red;
    [SerializeField] private float cautionThreshold = 0.6f; // 60%Ѫ�����±��
    [SerializeField] private float dangerThreshold = 0.3f;  // 30%Ѫ�����±��

    private void Start()
    {
        InitializeUI();
    }

    private void InitializeUI()
    {
        // ֱ��ʹ���Ѹ�ֵ��playerHealth
        if (playerHealth == null)
        {
            Debug.LogError("Player Health reference not assigned in inspector!");
            enabled = false; // ���ýű�
            return;
        }

        // ��ʼ�����¼�
        playerHealth.OnHealthChanged += UpdateHealthDisplay;

        // ǿ�Ƴ�ʼ����ʾ
        UpdateHealthDisplay(playerHealth.GetCurrentHealth(), playerHealth.GetMaxHealth());
    }

    private void UpdateHealthDisplay(float currentHealth, float maxHealth)
    {
        // ����Slider
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth / maxHealth;
        }

        // ����TMP�ı�����ʽ
        if (healthText != null)
        {
            // �ı�����
            healthText.text = $"HP: {Mathf.CeilToInt(currentHealth)}/{Mathf.CeilToInt(maxHealth)}";

            // ��̬��ɫ������Ѫ���ٷֱȣ�
            float healthPercent = currentHealth / maxHealth;
            if (healthPercent < dangerThreshold)
            {
                healthText.color = dangerColor;
                healthText.fontStyle = FontStyles.Bold; // Σ��ʱ�Ӵ�
            }
            else if (healthPercent < cautionThreshold)
            {
                healthText.color = cautionColor;
                healthText.fontStyle = FontStyles.Normal;
            }
            else
            {
                healthText.color = healthyColor;
                healthText.fontStyle = FontStyles.Normal;
            }

            // ��ѡ����Ӷ���Ч������Ѫ����ʱ��˸��
            if (healthPercent < dangerThreshold)
            {
                healthText.GetComponent<Animator>()?.SetTrigger("Danger");
            }
        }
    }

    private void OnDestroy()
    {
        if (playerHealth != null)
        {
            playerHealth.OnHealthChanged -= UpdateHealthDisplay;
        }
    }

}