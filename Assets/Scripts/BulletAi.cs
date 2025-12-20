using UnityEngine;

public class BulletAi : MonoBehaviour
{

    [SerializeField] public float bulletDamage = 10f;
    [SerializeField] private float bulletSpeed = 25;
    private Transform target;
    private TowerController owner;
    private Vector3 grovePosition;

    public void Init(TowerController tower, Vector3 grovePos)
    {
        owner = tower;
        grovePosition = grovePos;

        FindNearestEnemyToGrove();
    }

    void Update()
    {
        if(!target.CompareTag("Enemy"))
        {
            Destroy(gameObject);
        }
        
        transform.position = Vector3.MoveTowards(
            transform.position, target.transform.position, bulletSpeed * Time.deltaTime);
    }

    void FindNearestEnemyToGrove()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");

        if(enemies.Length == 0) return;

        Transform nearestEnemy = null;
        float shortestDistance = Mathf.Infinity;

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
        if (!other.gameObject.CompareTag("Enemy")) return;

        other.GetComponent<EnemyAi>()?.TakeDamage(bulletDamage);
        owner.OnBulletDestroyed();
        Destroy(gameObject);
    }
}
