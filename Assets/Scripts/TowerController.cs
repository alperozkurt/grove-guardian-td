using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    [SerializeField] private BulletController bulletController;
    public float fireRate = 1f;
    private float fireCountDown = 0f;
    [SerializeField] private int maxBulletCount;
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
}
