using System;
using TMPro;
using UnityEngine;

public class SpawnTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI waveTimerText;
    public event Action<float> OnTimeChanged;
    void Start()
    {
        OnTimeChanged += UpdateTimerUI;

        UpdateTimerUI(0);
    }

    // Called by spawner to update time 
    public void UpdateTime(float timeRemaining)
    {
        OnTimeChanged?.Invoke(timeRemaining);
    }

    private void UpdateTimerUI(float timeRemaining)
    {
        if(timeRemaining <= 0)
        {
            waveTimerText.text = "Wave In Progress";
        }
        else
        {
            waveTimerText.text = Mathf.Ceil(timeRemaining).ToString() + " Seconds Left To Next Wave";
        }
    }

    
}
