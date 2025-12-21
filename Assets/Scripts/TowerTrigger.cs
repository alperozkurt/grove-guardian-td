using UnityEngine;
using System.Collections;
using TMPro;

public class TowerTrigger : MonoBehaviour
{
    [SerializeField] private GameObject towerToCreate;
    [SerializeField] private int towerCost;
    [SerializeField] private AudioClip towerCreationAudio;
    private AudioSource audioSource;
    private Vector3 towerPosition;
    private bool used = false;
    private TextMeshProUGUI towerCostText;
    GroveController grove;
    void Start()
    {
        towerCost = 20;
        towerCostText = GetComponentInChildren<TextMeshProUGUI>();
        grove = FindAnyObjectByType<GroveController>();
        grove.TowerCountChanged += UpdateTowerCost;
        audioSource = GetComponent<AudioSource>();
    }
    void OnTriggerEnter(Collider other)
    {
        if(used) return;
        if(!other.gameObject.CompareTag("Player")) return;

        if (grove.TrySpendCoins(towerCost))
        {
        used = true;
        CreateTower();
        GetComponentInChildren<Canvas>().enabled = false;  
        }
    }
    void CreateTower()
    {
        towerPosition = new Vector3(
            gameObject.GetComponent<Transform>().position.x,
            -towerToCreate.transform.localScale.y,
            gameObject.GetComponent<Transform>().position.z);
        StartCoroutine(CreateTowerRoutine());   
    }
    IEnumerator CreateTowerRoutine()
    {
        grove.AddTowerCount();
        yield return new WaitForSeconds(1.5f);
        audioSource.PlayOneShot(towerCreationAudio);
        Instantiate(towerToCreate, towerPosition, Quaternion.identity);
    }

    void UpdateTowerCost(int towerCount)
    {
        towerCost += towerCount * 3;
        towerCostText.text = "Buy Tower " + towerCost.ToString() + " Coins";
    }

}
