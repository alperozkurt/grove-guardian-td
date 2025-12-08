using UnityEngine;

public class HealthBarBillboard : MonoBehaviour
{
    void Update()
    {
        transform.rotation = GameObject.FindGameObjectWithTag("MainCamera").transform.rotation;
    }
}