using System;
using TMPro;
using UnityEngine;

public class SpawnTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waveTimerText;
    public event Action<float, bool> OnTimeChanged;
    void Start()
    {
        OnTimeChanged += UpdateTimerUI;

        UpdateTimerUI(0);
    }

    // Called by spawner to update time 
    public void UpdateTime(float timeRemaining, bool lastWave = false) 
    {
        OnTimeChanged?.Invoke(timeRemaining, lastWave);
    }

    private void UpdateTimerUI(float timeRemaining, bool lastWave = false)
    {
        if (lastWave)
        {
            waveTimerText.text = "Boss Wave";
            return;
        }
        if(timeRemaining <= 0)
        {
            waveTimerText.text = "Wave In Progress";
        }
        else
        {
            waveTimerText.text = Mathf.Ceil(timeRemaining).ToString("F0") + " Seconds Left To Next Wave";
        }
    } 
}
