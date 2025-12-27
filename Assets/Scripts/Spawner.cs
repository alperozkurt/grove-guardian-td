using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    [SerializeField] private InputAction skipWaveAction;
    [SerializeField] private Button skipButton;
    [SerializeField] private AudioClip waveStartAudio;
    private AudioSource audioSource;
    private BoxCollider spawnArea;
    private EnemyCounter enemyCounter;
    private SpawnTimer spawnTimer;
    public List<Wave> waves;
    private void OnEnable() => skipWaveAction.Enable();
    private void OnDisable() => skipWaveAction.Disable();

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        spawnArea = GetComponent<BoxCollider>();
        spawnArea.isTrigger = true;

        enemyCounter = FindFirstObjectByType<EnemyCounter>();

        spawnTimer = FindFirstObjectByType<SpawnTimer>();

        if(skipButton != null) skipButton.interactable = false;
        
        StartCoroutine(SummonAllWaves());
    }

    IEnumerator SummonAllWaves()
    {
        // Initial wait
        yield return StartCoroutine(WaitForWaveTimer(15f)); // initial wait
        
        for(int i = 0; i < waves.Count; i ++)
        {
            Wave currentWave = waves[i];

            if(skipButton != null) skipButton.interactable = false;

            if(audioSource != null) audioSource.PlayOneShot(waveStartAudio);

            if(i == waves.Count - 1)
            {
                // Boss wave
                GameObject bossPrefab = currentWave.waveGroups[0].enemyPrefab;
                SpawnBoss(bossPrefab);
            }
            else
            {
                // Normal wave spawn
                foreach(WaveGroup group in currentWave.waveGroups)
                {
                    
                    for(int j = 0; j < group.spawnCount; j++)
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
    }

    IEnumerator WaitForWaveTimer(float duration)
    {
        if(skipButton != null) skipButton.interactable = true;

        float timer = duration;

        while(timer > 0)
        {
            // If pressed skip wave wait
            if (skipWaveAction.WasPressedThisFrame())
            {
                timer = 0;
            }

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

        // Disable skipButton when timer is over
        skipButton.interactable = false;
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

    void SpawnBoss(GameObject boss)
    {
        Vector3 spawnPos = GetBossPosition();
        Instantiate(boss, spawnPos, Quaternion.identity);
        enemyCounter.OnEnemySpawn();
        spawnTimer.UpdateTime(0, true);
        enemyCounter.SetAllWavesSpawned();
    }

    Vector3 GetBossPosition()
    {
        float x = transform.position.x;
        float y = 1.5f;
        float z = transform.position.z;
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
