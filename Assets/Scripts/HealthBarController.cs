using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] private Image fillImage;
    [SerializeField] private TextMeshProUGUI healthText;
    private float maxHealthVal;

    public void Setup(float maxHealth)
    {
        this.maxHealthVal = maxHealth;
        UpdateHealthUI(maxHealth);
    }

    public void UpdateHealthUI(float currentHealth)
    {
        fillImage.fillAmount = currentHealth / maxHealthVal;
        healthText.text = $"{currentHealth}/{maxHealthVal}";
    }

}
