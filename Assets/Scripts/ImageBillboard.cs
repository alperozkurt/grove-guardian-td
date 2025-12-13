using UnityEngine;

public class ImageBillboard : MonoBehaviour
{
    void Update()
    {
        transform.rotation = GameObject.FindGameObjectWithTag("MainCamera").transform.rotation;
    }
}