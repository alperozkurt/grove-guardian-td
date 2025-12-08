using UnityEngine;

public class BulletAi : MonoBehaviour
{

    [SerializeField] public float bulletDamage;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private GameObject grove;
    private Transform target;
    void Start()
    {
        FindNearestEnemyToGrove();
    }

    void Update()
    {
        if(target == null)
        {
            FindNearestEnemyToGrove();
        }
        transform.position = Vector3.MoveTowards(
            transform.position, target.transform.position, bulletSpeed * Time.deltaTime);
    }

    void FindNearestEnemyToGrove()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        Transform nearestEnemy = null;
        float shortestDistance = Mathf.Infinity;
        Vector3 grovePosition = grove.transform.position;

        if(enemies == null) return;

        foreach(GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(enemy.transform.position, grovePosition);

            if(distanceToEnemy < shortestDistance)
            {
                nearestEnemy = enemy.transform;
                shortestDistance = distanceToEnemy;
            }
        }
        target = nearestEnemy;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            EnemyAi enemyAi = other.GetComponent<EnemyAi>();
            enemyAi.TakeDamage(bulletDamage);
            Destroy(gameObject);
        }
    }
}
