using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    [SerializeField] private float speed = 5f;
    [SerializeField] private float maxHealth;
    [SerializeField] private float damageOnImpact;
    [SerializeField] private int coinOnDeath = 2;
    [SerializeField] private EnemyHealthBar healthBar;
    float currentHealth;
    Vector3 target;
    GroveController grove;
    private bool isDead = false;
    Animator animator;

    void Start()
    {
        grove = FindFirstObjectByType<GroveController>();
        gameObject.transform.LookAt(grove.transform);
        target = new Vector3(grove.transform.position.x, gameObject.transform.position.y, grove.transform.position.z);
        currentHealth = maxHealth;
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
        animator = gameObject.GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if(isDead) return;

        if(grove != null)
        transform.position = Vector3.MoveTowards(
            transform.position, target, speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if(!other.gameObject.CompareTag("Base")) return;
        if(grove != null)
        {
            grove.AddCoins(coinOnDeath);
            grove.DealDamageToBase(damageOnImpact);
        }
        Destroy(gameObject);
        
    }
    public void TakeDamage(float damage)
    {
        if(isDead) return;

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
        if(isDead) return;

        isDead = true;

        GetComponentInChildren<Canvas>().enabled = false;
        GetComponent<CapsuleCollider>().enabled = false;
        gameObject.tag = "Untagged";

        try
        {
            animator.SetTrigger("Die");
        }
        catch
        {
            Debug.Log("GameObejct does not have a death aniamtion");
        }
       
        if(grove != null)
        {
            grove.AddCoins(coinOnDeath);
        }

        Destroy(gameObject, 2f);
    }
}
