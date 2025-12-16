using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<Wave> waves;
    void Start()
    {
        StartCoroutine(SummonAllWaves());
    }
    IEnumerator SummonAllWaves()
    {
        foreach(Wave currentWave in waves)
        {
            Debug.Log("Starting: " + currentWave.waveName);
            foreach(WaveGroup group in currentWave.waveGroups)
            {
                for(int i = 0; i < group.spawnCount; i++)
                {
                    SpawnEnemy(group.enemyPrefab);

                    yield return new WaitForSeconds(group.spawnRate);
                }

                if(group.delayAfterGroup > 0)
                {
                    yield return new WaitForSeconds(group.delayAfterGroup);
                }
            }

            yield return new WaitForSeconds(currentWave.timeToNextWave);
        }
    }

    void SpawnEnemy(GameObject enemyPrefab)
    {
        Instantiate(enemyPrefab, gameObject.transform.position, Quaternion.identity);
    }
}

[System.Serializable]
public class WaveGroup
{   
    public GameObject enemyPrefab;
    public int spawnCount;
    public float spawnRate;
    public float delayAfterGroup;
}

[System.Serializable]
public class Wave
{
    public string waveName;
    public List<WaveGroup> waveGroups;
    public float timeToNextWave;
}
