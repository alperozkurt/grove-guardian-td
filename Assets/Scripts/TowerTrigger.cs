using UnityEngine;
using System.Collections;
using TMPro;

public class TowerTrigger : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject towerToCreate;
    [SerializeField] private AudioClip towerCreationAudio;
    [SerializeField] private int baseTowerCost = 15;
    private int currentCost;
    private AudioSource audioSource;
    private bool isUsed = false;
    private TextMeshProUGUI towerCostText;
    GroveController grove;
    void Start()
    {
        currentCost = baseTowerCost;

        towerCostText = GetComponentInChildren<TextMeshProUGUI>();
        audioSource = GetComponent<AudioSource>();

        grove = FindAnyObjectByType<GroveController>();

        if(grove != null)
        {
            grove.TowerCountChanged += UpdateTowerCost;
            UpdateTowerCost(grove.GetTowerCount());
        }
        
        
    }
    void OnTriggerEnter(Collider other)
    {
        if(isUsed || !other.gameObject.CompareTag("Player")) return;

        if (grove != null && grove.TrySpendCoins(currentCost))
        {
            isUsed = true;
            StartCoroutine(CreateTowerRoutine());

            GetComponentInChildren<Canvas>().enabled = false;  
        }
    }
    IEnumerator CreateTowerRoutine()
    {
        grove.AddTowerCount();
        Vector3 spawnPos = new Vector3(
            transform.position.x,
            -towerToCreate.transform.localScale.y,
            transform.position.z);

        yield return new WaitForSeconds(1.5f);

        if(audioSource && towerCreationAudio)
        {
            audioSource.PlayOneShot(towerCreationAudio);
        }
        
        Instantiate(towerToCreate, spawnPos, Quaternion.identity);

        Destroy(gameObject, 1.2f);
    }

    void UpdateTowerCost(int towerCount)
    {
        currentCost = baseTowerCost + (towerCount * 6);

        if(towerCostText != null)
        {
            towerCostText.text = $"Buy Tower\n{currentCost} Coins";
        }
    }

    void OnDestroy()
    {
        if (grove != null)
        {
            grove.TowerCountChanged -= UpdateTowerCost;
        }
    }

}
