using System.Collections;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    [SerializeField] private BulletController bulletController;
    public float fireRate = 1f;
    private float fireCountDown = 0f;
    [SerializeField] private int maxBulletCount = 3;
    private float targetY;

    void Start()
    {    
        StartCoroutine(FinishCreating());
    }
    void Update()
    {
        int currentBulletCount = GameObject.FindGameObjectsWithTag("Bullet").Length;

        if(GameObject.FindWithTag("Enemy") == null) return;

        if(fireCountDown <= 0f && currentBulletCount < maxBulletCount)
        {
            bulletController.SpawnBullet();
            fireCountDown = 1f / fireRate;
        }
        fireCountDown -= Time.deltaTime;
    }
    IEnumerator FinishCreating()
    {
        targetY = 0.5f;
        while(transform.position.y < targetY)
        {
            float newY = transform.position.y + (5f * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            yield return null;
        }
        transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
        yield return null;
    }
}
