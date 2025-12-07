using UnityEngine;

public class HealthBarBillboard : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;

    void Start()
    {
        
    }
    void LateUpdate()
    {
        transform.rotation = mainCamera.GetComponent<Transform>().rotation;;
    }
}