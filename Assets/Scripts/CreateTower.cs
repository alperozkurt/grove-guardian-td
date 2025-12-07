using System.Collections;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private GameObject towerToCreate;
    private Vector3 towerPosition;
    void Start()
    {
        towerPosition = new Vector3(
            gameObject.GetComponent<Transform>().position.x,
            -5,
            gameObject.GetComponent<Transform>().position.z);
        StartCoroutine(CreateTower());
    }

    IEnumerator CreateTower()
    {
        yield return new WaitForSeconds(3f);
        Instantiate(towerToCreate, towerPosition, Quaternion.identity);
    }
}
