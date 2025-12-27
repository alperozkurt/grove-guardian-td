using UnityEngine;
using TMPro;

public class CoinDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;
    private GroveController grove;

    void Start()
    {
        grove = FindFirstObjectByType<GroveController>();
        UpdateCoinUI(grove.coin);
        grove.CoinChanged += UpdateCoinUI;
    }

    void OnDisable()
    {
        grove.CoinChanged -= UpdateCoinUI;
    }

    private void UpdateCoinUI(int newCoinValue)
    {
        coinText.text = newCoinValue.ToString();
    }
}