using System.Collections;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    [SerializeField] private GameObject towerToCreate;

    private Vector3 towerPosition;
    private float towerScale;

    void Start()
    {
        towerScale = towerToCreate.GetComponent<Transform>().localScale.y;
        towerPosition = new Vector3(
            gameObject.GetComponent<Transform>().position.x,
            -(towerScale - 1)/2,
            gameObject.GetComponent<Transform>().position.z);
        StartCoroutine(CreateTower());
    }

    IEnumerator CreateTower()
    {
        yield return new WaitForSeconds(3f);
        Instantiate(towerToCreate, towerPosition, Quaternion.identity);
    }
}
