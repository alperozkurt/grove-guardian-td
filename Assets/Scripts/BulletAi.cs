using UnityEngine;

public class BulletAi : MonoBehaviour
{

    [SerializeField] public float bulletDamage = 10f;
    [SerializeField] private float bulletSpeed = 25;
    private Transform target;
    private TowerController owner;

    public void Init(TowerController tower)
    {
        owner = tower;
        target = FindNearestEnemyToTower();
    }

    void Update()
    {
        if(!target.CompareTag("Enemy"))
        {
            owner.OnBulletDestroyed();
            Destroy(gameObject); 
        }
        
        transform.position = Vector3.MoveTowards(
            transform.position, target.transform.position, bulletSpeed * Time.deltaTime);
    }

    Transform FindNearestEnemyToTower()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if(enemies.Length == 0) return null;

        Transform nearestEnemy = null;
        float shortestDistance = Mathf.Infinity;

        foreach(GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector3.Distance(enemy.transform.position, gameObject.transform.position);

            if(distanceToEnemy < shortestDistance)
            {
                nearestEnemy = enemy.transform;
                shortestDistance = distanceToEnemy;
            }
        }
        return nearestEnemy;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Enemy")) return;

        other.GetComponent<EnemyAi>()?.TakeDamage(bulletDamage);
        owner.OnBulletDestroyed();
        Destroy(gameObject);
    }
}
