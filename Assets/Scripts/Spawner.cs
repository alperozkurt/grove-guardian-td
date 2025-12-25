using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private AudioClip waveStartAudio;
    private AudioSource audioSource;
    private BoxCollider spawnArea;
    public List<Wave> waves;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        spawnArea = GetComponent<BoxCollider>();
        spawnArea.isTrigger = true;
        
        StartCoroutine(SummonAllWaves());
    }
    IEnumerator SummonAllWaves()
    {
        // Initial wait
        yield return new WaitForSeconds(15f);
        if(audioSource != null)
        {
            audioSource.PlayOneShot(waveStartAudio);
        }
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
        Debug.Log("Level Complete!");
    }

    void SpawnEnemy(GameObject enemyPrefab)
    {
        Vector3 spawnPos = GetRandomPosition();
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
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
