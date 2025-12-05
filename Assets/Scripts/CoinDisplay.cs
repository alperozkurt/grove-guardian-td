using UnityEngine;
using TMPro; // Required for TextMeshPro

public class CoinDisplay : MonoBehaviour
{
    // Drag your Text object here in the Inspector
    [SerializeField] private TextMeshProUGUI coinText;
    
    // Internal variable to track coins
    private int currentCoins = 0;

    void Start()
    {
        UpdateCoinUI();
    }

    // Call this function whenever the player picks up a coin
    public void AddCoins(int amount)
    {
        currentCoins += amount;
        UpdateCoinUI();
    }

    private void UpdateCoinUI()
    {
        // Updates the text to match the number
        coinText.text = currentCoins.ToString();
    }
}