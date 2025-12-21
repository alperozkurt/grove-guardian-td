using System.Collections;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    [SerializeField] private BulletController bulletController;
    [SerializeField] private int maxBulletCount = 3;
    public float defaultFireRate = 1f;
    public float fireRate;
    private float fireCountDown = 0f;
    private float targetY;
    private bool isReady = false;
    public int currentBulletCount;
    private bool isBoosted = false;
    private GameObject activeBoostEffect;

    void Start()
    {
        fireRate = defaultFireRate;     
        StartCoroutine(FinishCreating());
    }
    void Update()
    {
        if(!isReady) return;
        
        if(GameObject.FindWithTag("Enemy") == null) return;

        if(fireCountDown <= 0f && currentBulletCount < maxBulletCount)
        {
            OnBulletCreated();
            bulletController.SpawnBullet(this);
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
        isReady = true;
    }

    public void OnBulletCreated()
    {
        currentBulletCount++;
    }

    public void OnBulletDestroyed()
    {
        currentBulletCount--;
    }

    public void ApplyBoost(float boostRate, float duration, GameObject effectPrefab)
    {
        if (isBoosted)
        {
            StopCoroutine("IncreaseFireRateCoroutine");
            if(activeBoostEffect != null) Destroy(activeBoostEffect);
        }
        StartCoroutine(IncreaseFireRateCoroutine(boostRate,duration, effectPrefab));
    }

    IEnumerator IncreaseFireRateCoroutine(float boostRate, float duration, GameObject effectPrefab)
    {
        isBoosted = true;
        fireRate = defaultFireRate * boostRate;
        if(effectPrefab != null)
        {
            activeBoostEffect = Instantiate(effectPrefab, transform.position, transform.rotation);
        }
        yield return new WaitForSeconds(duration);
        if(activeBoostEffect != null) Destroy(activeBoostEffect);
        fireRate = defaultFireRate;
        isBoosted = false;
    }
}
