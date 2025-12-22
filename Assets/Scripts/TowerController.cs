using System.Collections;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    [Header("Settings")] 
    [SerializeField] private float range = 25f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private int maxBulletCount = 3;
    [SerializeField] private BulletController bulletController;

    private float fireCountDown = 0f;
    private float defaultFireRate;
    private float defaultRange;
    private Transform currentTarget;
    public int currentBulletCount;
    private bool isReady = false;
    private GameObject activeBoostEffect;
    private Coroutine boostCoroutine;

    void Start()
    {
        defaultFireRate = fireRate;
        defaultRange = range;   
        StartCoroutine(FinishCreating());
    }
    void Update()
    {
        if(!isReady) return;
        
        UpdateTarget();

        if (currentTarget != null && fireCountDown <= 0f && currentBulletCount < maxBulletCount)
        {
            FireBullet();
            fireCountDown = 1f / fireRate;
        }
        fireCountDown -= Time.deltaTime;
    }
    IEnumerator FinishCreating()
    {
        float targetY = -0.5f;
        while(transform.position.y < targetY)
        {
            float newY = transform.position.y + (5f * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, newY, transform.position.z);
            yield return null;
        }
        transform.position = new Vector3(transform.position.x, targetY, transform.position.z);
        yield return null;
        isReady = true;
    }

    void UpdateTarget()
    {
        if(currentTarget != null)
        {
            float distance = Vector3.Distance(transform.position, currentTarget.position);
            if (distance > range || !currentTarget.gameObject.activeInHierarchy || !currentTarget.CompareTag("Enemy"))
            {
                currentTarget = null;
            }
        }

        if(currentTarget == null)
        {
            FindNearestEnemy();
        }
    }

    void FindNearestEnemy()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, range, enemyLayer);

        float shortestDistance = Mathf.Infinity;
        Transform nearestEnemy = null;

        foreach(Collider enemy in enemies)
        {
            if (!enemy.gameObject.CompareTag("Enemy")) continue;

            float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);

            if(distanceToEnemy < shortestDistance)
            {
                shortestDistance = distanceToEnemy;
                nearestEnemy = enemy.transform;
            }
        }

        currentTarget = nearestEnemy;
    }

    void FireBullet()
    {
        OnBulletCreated();
        bulletController.SpawnBullet(this, currentTarget);
    }

    public void OnBulletCreated() => currentBulletCount++;
    public void OnBulletDestroyed() => currentBulletCount--;

    public void ApplyBoost(float boostRate, float duration, GameObject effectPrefab)
    {
        if (boostCoroutine != null) StopCoroutine(boostCoroutine);
        if (activeBoostEffect != null) Destroy(activeBoostEffect);

        boostCoroutine = StartCoroutine(IncreaseFireRateCoroutine(boostRate,duration,effectPrefab));
    }

    IEnumerator IncreaseFireRateCoroutine(float boostRate, float duration, GameObject effectPrefab)
    {
        fireRate = defaultFireRate * boostRate;
        range = defaultRange * boostRate;
        if(effectPrefab != null)
        {
            activeBoostEffect = Instantiate(effectPrefab, transform.position, transform.rotation);
        }
        yield return new WaitForSeconds(duration);
        fireRate = defaultFireRate;
        range = defaultRange;
        if(activeBoostEffect != null) Destroy(activeBoostEffect);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}
