using System;
using TMPro;
using UnityEngine;

public class EnemyCounter : MonoBehaviour
{   
    [SerializeField] private TextMeshProUGUI enemyCountText;
    private int enemyCount;
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
    }

    private void UpdateEnemyCountUI(int enemyCount)
    {
        enemyCountText.text = enemyCount.ToString() + " Enemies remain";
    }
}
