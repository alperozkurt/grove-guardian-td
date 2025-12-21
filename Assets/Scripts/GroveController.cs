using System;
using UnityEngine;

public class GroveController : MonoBehaviour
{
    [SerializeField] public float maxHealth;
    [SerializeField] public int coin;
    [SerializeField] private HealthBarController healthUI;
    public float currentHealth;
    public int towerCount = 0;
    public event Action<int> CoinChanged;
    public event Action<int> TowerCountChanged;

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

        CoinChanged?.Invoke(coin);
        TowerCountChanged?.Invoke(towerCount);
    }

    public void DealDamageToBase(float damage)
{
    currentHealth -= damage;
    currentHealth = Mathf.Max(currentHealth, 0);

    if (healthUI != null)
        healthUI.UpdateHealthUI(currentHealth);

    if (currentHealth <= 0)
        Debug.Log("Game Over!");
}

    public void AddCoins(int amount)
    {
        coin += amount;
        CoinChanged?.Invoke(coin);
    }

    public bool TrySpendCoins(int amount)
    {
        if (coin < amount) return false;

        coin -= amount;
        CoinChanged?.Invoke(coin);
        return true;
    }

    public void AddTowerCount()
    {
        towerCount ++;
        TowerCountChanged?.Invoke(towerCount);
    }
}
