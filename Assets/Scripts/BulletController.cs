using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    public BulletAi SpawnBullet(TowerController owner, Transform target)
    {
        GameObject gameObject = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        BulletAi bullet = gameObject.GetComponent<BulletAi>();
        bullet.Init(owner, target);

        return bullet;
    }
}
