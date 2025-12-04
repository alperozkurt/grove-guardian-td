using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private float damageOnImpact;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Base"))
        {
            Destroy(gameObject);
        }
    }

}
