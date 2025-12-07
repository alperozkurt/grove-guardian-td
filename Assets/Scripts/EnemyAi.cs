using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float maxHealth;
    [SerializeField] private float damageOnImpact;
    [SerializeField] private int coinOnDeath = 2;
    [SerializeField] private EnemyHealthBar healthBar;
    float currentHealth;

    void Start()
    {   
        currentHealth = maxHealth;
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
    }

    void Update()
    {
        if(currentHealth <= 0)
        {
            Die();
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

            Die();
        }
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        currentHealth = Mathf.Max(currentHealth, 0);

        if(healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth,maxHealth);
        }
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        CoinDisplay hud = FindFirstObjectByType<CoinDisplay>();
        if(hud != null)
        {
            hud.AddCoins(coinOnDeath);
        }
        Destroy(gameObject);
    }
}
