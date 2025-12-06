using UnityEngine;

public class GroveController : MonoBehaviour
{
    [SerializeField] public float maxHealth;
    [SerializeField] public float coin;
    [SerializeField] private HealthBarController healthUI;
    public float currentHealth;  

    void Start()
    {
        currentHealth = maxHealth;
        if(healthUI == null)
        {
            healthUI = FindFirstObjectByType<HealthBarController>();
        }
        if(healthUI != null)
        {
            healthUI.Setup(maxHealth);
        }
    }

    public void DealDamageToBase(float damage)
    {
        currentHealth -= damage;
        healthUI.UpdateHealthUI(currentHealth);
        if(currentHealth <= 0)
        {
            Debug.Log("Game Over!");
        }
    }
}
