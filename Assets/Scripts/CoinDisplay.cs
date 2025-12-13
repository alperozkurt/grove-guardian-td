using UnityEngine;
using TMPro;

public class CoinDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private GameObject grove;
    private int currentCoins;

    void Start()
    {
        currentCoins = grove.GetComponent<GroveController>().coin;
        UpdateCoinUI();
    }

    public void AddCoins(int amount)
    {
        currentCoins += amount;
        UpdateCoinUI();
    }

    public void RemoveCoins(int amount)
    {
        currentCoins -= amount;
        UpdateCoinUI();
    }

    private void UpdateCoinUI()
    {
        coinText.text = currentCoins.ToString();
    }
}