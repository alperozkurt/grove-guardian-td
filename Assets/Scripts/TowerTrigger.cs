using UnityEngine;
using System.Collections;

public class TowerTrigger : MonoBehaviour
{
    [SerializeField] private GameObject towerToCreate;
    [SerializeField] private int towerCost;
    private Vector3 towerPosition;
    void OnTriggerEnter(Collider other)
    {
        int coin = FindAnyObjectByType<GroveController>().coin;
        if (other.gameObject.CompareTag("Player") && coin >= towerCost)
        { 
            CoinDisplay hud = FindFirstObjectByType<CoinDisplay>();
            if(hud != null)
            {
                hud.RemoveCoins(towerCost);
            }   
            CreateTower();
            gameObject.GetComponentInChildren<Canvas>().enabled = false;
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
        yield return new WaitForSeconds(3f);
        Instantiate(towerToCreate, towerPosition, Quaternion.identity);
    }
}
