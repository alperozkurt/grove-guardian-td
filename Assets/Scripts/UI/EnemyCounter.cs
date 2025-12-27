using System;
using TMPro;
using UnityEngine;

public class EnemyCounter : MonoBehaviour
{   
    [SerializeField] private TextMeshProUGUI enemyCountText;
    [SerializeField] private GameObject victoryPanel;
    private int enemyCount;
    private bool allWavesSpawned = false;
    public event Action<int> EnemyCountChanged;
    void Start()
    {
        enemyCount = 0; // initial enemy enemyCount
        if(victoryPanel != null) victoryPanel.SetActive(false); // ensure hidden

        EnemyCountChanged?.Invoke(enemyCount);
        EnemyCountChanged += UpdateEnemyCountUI;
    }

    public void OnEnemySpawn()
    {
        enemyCount++;
        EnemyCountChanged?.Invoke(enemyCount);
    }

    public void OnEnemyDeath()
    {
        enemyCount--;
        EnemyCountChanged?.Invoke(enemyCount);

        CheckVictoryCondition();
    }

    public void SetAllWavesSpawned()
    {
        allWavesSpawned = true;
        CheckVictoryCondition();
    }

    private void CheckVictoryCondition()
    {
        if(allWavesSpawned && enemyCount < 0)
        {
            victoryPanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    private void UpdateEnemyCountUI(int enemyCount)
    {
        enemyCountText.text = enemyCount.ToString() + " Enemies remain";
    }
}
