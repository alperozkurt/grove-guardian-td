using UnityEngine;

public class BulletAi : MonoBehaviour
{
    [SerializeField] public float bulletDamage = 10f;
    [SerializeField] private float bulletSpeed = 25f;
    [SerializeField] private float maxLifeTime = 5f; // Failsafe timer

    private Transform target;
    private TowerController owner;
    private Vector3 lastKnownPosition;

    public void Init(TowerController tower, Transform newTarget)
    {
        owner = tower;
        target = newTarget;
        
        if(target != null) lastKnownPosition = target.position; 
        
        // Failsafe: Destroy bullet after 5 seconds no matter what
        // This prevents ghosts if physics glitched or bullet flew off map
        Invoke(nameof(ForceDestroy), maxLifeTime);
    }

    void Update()
    {
        // 1. Update target position if it's still alive
        if (target != null)
        {
            lastKnownPosition = target.position;
        }

        // 2. Move
        float step = bulletSpeed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, lastKnownPosition, step);

        // 3. CHECK FOR GHOSTING
        // If target is dead (null) AND we have reached the last known position...
        if (target == null && transform.position == lastKnownPosition)
        {
            // ...we hit nothing (ghost bullet). Destroy it now.
            RecycleBullet();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.gameObject.CompareTag("Enemy")) return;

        EnemyAi enemy = other.GetComponent<EnemyAi>();
        
        // Apply damage if enemy script exists
        if (enemy != null)
        {
            enemy.TakeDamage(bulletDamage);
        }
        
        // Always destroy on impact with enemy
        RecycleBullet();
    }

    // Centralized function to clean up
    void RecycleBullet()
    {
        CancelInvoke(nameof(ForceDestroy)); // Stop the failsafe timer
        
        // IMPORTANT: Tell the tower the bullet is gone, or it stops firing
        if (owner != null) 
        {
            owner.OnBulletDestroyed();
        }
        
        Destroy(gameObject);
    }

    // Called by Invoke if bullet lives too long
    void ForceDestroy()
    {
        RecycleBullet();
    }
}