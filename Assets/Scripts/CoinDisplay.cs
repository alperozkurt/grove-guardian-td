using UnityEngine;
using TMPro;

public class CoinDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private GroveController grove;
    void OnEnable()
    {
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