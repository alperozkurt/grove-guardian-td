using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private AudioClip waveStartAudio;
    private AudioSource audioSource;
    private BoxCollider spawnArea;
    private EnemyCounter enemyCounter;
    private SpawnTimer spawnTimer;
    public List<Wave> waves;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        spawnArea = GetComponent<BoxCollider>();
        spawnArea.isTrigger = true;

        enemyCounter = FindFirstObjectByType<EnemyCounter>();

        spawnTimer = FindFirstObjectByType<SpawnTimer>();
        
        StartCoroutine(SummonAllWaves());
    }
    IEnumerator SummonAllWaves()
    {
        // Initial wait
        yield return StartCoroutine(WaitForWaveTimer(15f)); // initial wait
        
        foreach(Wave currentWave in waves)
        {
            if(audioSource != null)
            {
                audioSource.PlayOneShot(waveStartAudio);
            }
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

            yield return StartCoroutine(WaitForWaveTimer(currentWave.timeToNextWave));
        }
    }

    IEnumerator WaitForWaveTimer(float duration)
    {
        float timer = duration;

        while(timer > 0)
        {
            timer -= Time.deltaTime;

            if(spawnTimer != null)
            {
                spawnTimer.UpdateTime(timer);
            }

            yield return null;
        }

        if(spawnTimer != null)
        {
            spawnTimer.UpdateTime(0);
        }
    }

    void SpawnEnemy(GameObject enemyPrefab)
    {
        Vector3 spawnPos = GetRandomPosition();
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        enemyCounter.OnEnemySpawn();
    }

    Vector3 GetRandomPosition()
    {
        Bounds bounds = spawnArea.bounds;
        float x = transform.position.x;
        float y = transform.position.y;
        float z = Random.Range(bounds.min.z, bounds.max.z);

        return new Vector3(x, y, z);
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
