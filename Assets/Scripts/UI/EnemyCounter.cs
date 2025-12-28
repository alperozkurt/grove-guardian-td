using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyCounter : MonoBehaviour
{   
    [SerializeField] private TextMeshProUGUI enemyCountText;
    private int enemyCount;
    private bool allWavesSpawned = false;
    public bool isBossDead = false;
    public event Action<int> EnemyCountChanged;
    void Start()
    {
        enemyCount = 0; // initial enemy enemyCount

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
        if(allWavesSpawned && enemyCount <= 0 && isBossDead)
        {
            StartCoroutine(nameof(StartVictoryAnimations));
        }
    }

    IEnumerator StartVictoryAnimations()
    {
        yield return new WaitForSeconds(3.5f);
        VictoryPanel victoryPanel = FindFirstObjectByType<VictoryPanel>();
        victoryPanel.SetVictory();
    }

    private void UpdateEnemyCountUI(int enemyCount)
    {
        enemyCountText.text = enemyCount.ToString() + " Enemies remain";
    }
}
