using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float maxHealth;
    [SerializeField] private float damageOnImpact;
    [SerializeField] private int coinOnDeath = 2;
    float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }

    void Update()
    {
        if(currentHealth <= 0)
        {
            CoinDisplay hud = FindFirstObjectByType<CoinDisplay>();
            if(hud != null)
            {
                hud.AddCoins(coinOnDeath);
            }
            Destroy(gameObject);
        }

        transform.position = Vector3.MoveTowards(
            transform.position, target.transform.position, speed * Time.deltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Base"))
        {
            GroveController grove = collision.gameObject.GetComponent<GroveController>();

            if(grove != null)
            {
                grove.DealDamageToBase(damageOnImpact);
            }

            CoinDisplay hud = FindFirstObjectByType<CoinDisplay>();
            if(hud != null)
            {
                hud.AddCoins(coinOnDeath);
            }

            Destroy(gameObject);
        }
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
    }
}
