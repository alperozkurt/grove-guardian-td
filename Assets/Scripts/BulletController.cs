using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private GameObject bullet;
    public void SpawnBullet()
    {
        Instantiate(bullet, gameObject.transform.position, Quaternion.identity);
    }
}
