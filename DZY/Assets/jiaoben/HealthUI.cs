using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthUI : MonoBehaviour
{
    [Header("Player Reference")]
    [Tooltip("拖拽Player对象到这里")]
    [SerializeField] private Health playerHealth;  // 编辑器拖拽赋予对象

    [Header("UI References")]
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI healthText;

    [Header("Style Settings")]
    [SerializeField] private Color healthyColor = Color.green;
    [SerializeField] private Color cautionColor = Color.yellow;
    [SerializeField] private Color dangerColor = Color.red;
    [SerializeField] private float cautionThreshold = 0.6f; // 60%血量以下变黄
    [SerializeField] private float dangerThreshold = 0.3f;  // 30%血量以下变红

    private void Start()
    {
        InitializeUI();
    }

    private void InitializeUI()
    {
        // 直接使用已赋值的playerHealth
        if (playerHealth == null)
        {
            Debug.LogError("Player Health reference not assigned in inspector!");
            enabled = false; // 禁用脚本
            return;
        }

        // 初始订阅事件
        playerHealth.OnHealthChanged += UpdateHealthDisplay;

        // 强制初始化显示
        UpdateHealthDisplay(playerHealth.GetCurrentHealth(), playerHealth.GetMaxHealth());
    }

    private void UpdateHealthDisplay(float currentHealth, float maxHealth)
    {
        // 更新Slider
        if (healthSlider != null)
        {
            healthSlider.value = currentHealth / maxHealth;
        }

        // 更新TMP文本及样式
        if (healthText != null)
        {
            // 文本内容
            healthText.text = $"HP: {Mathf.CeilToInt(currentHealth)}/{Mathf.CeilToInt(maxHealth)}";

            // 动态颜色（根据血量百分比）
            float healthPercent = currentHealth / maxHealth;
            if (healthPercent < dangerThreshold)
            {
                healthText.color = dangerColor;
                healthText.fontStyle = FontStyles.Bold; // 危险时加粗
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

            // 可选：添加动画效果（如血量低时闪烁）
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