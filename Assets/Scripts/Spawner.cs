using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<Wave> waves;
    public float timeBetweenWaves = 10f;


    void Start()
    {
        StartCoroutine(SummonAllWaves());
    }
    IEnumerator SummonAllWaves()
    {
        yield return new WaitForSeconds(timeBetweenWaves);
        foreach(Wave currentWave in waves)
        {
            Debug.Log("Current wave: " + currentWave.waveCount);
            for(int i = 0; i < currentWave.spawnCount; i++)
            {
                SpawnEnemy(currentWave.enemyPrefab);
                yield return new WaitForSeconds(currentWave.cooldown);
            }
            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    void SpawnEnemy(GameObject enemyPrefab)
    {
        Instantiate(enemyPrefab, gameObject.transform.position, Quaternion.identity);
    }
}

[System.Serializable]
public class Wave
{
    public int waveCount;
    public GameObject enemyPrefab;
    public int spawnCount;
    public float cooldown;
}
