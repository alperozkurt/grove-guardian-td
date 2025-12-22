using System.Collections;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    [Header("Enemy Stats")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float maxHealth;
    [SerializeField] private float damageOnImpact;
    [SerializeField] private int coinOnDeath = 2;
    
    [Header("References")]
    [SerializeField] private EnemyHealthBar healthBar;
    [SerializeField] private AudioClip deathSound;

    // State Variables
    float currentHealth;
    private float defaultSpeed;
    private bool isSlowed;
    private bool isDead = false;

    // Cache
    Transform targetDestination;
    GroveController grove; 
    Animator animator;
    new Animation animation;
    private GameObject activeSlowEffect;
    private AudioSource audioSource;
    Collider enemyCollider;

    void Start()
    {
        defaultSpeed = speed;
        currentHealth = maxHealth;

        enemyCollider = GetComponent<Collider>();
        audioSource = GetComponent<AudioSource>();
        grove = FindFirstObjectByType<GroveController>();

        if (healthBar != null)
        {
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
        }
        
        if (grove != null)
        {
            targetDestination = grove.transform;
            transform.LookAt(new Vector3(targetDestination.position.x, transform.position.y, targetDestination.position.z));
        }

        
        animator = GetComponentInChildren<Animator>();
        if(animator == null)
        {
            animation = GetComponent<Animation>();
        }
    }

    void Update()
    {
        if(isDead || targetDestination == null) return;

        Vector3 destination = new Vector3(targetDestination.position.x, transform.position.y, targetDestination.position.z);
        
        transform.position = Vector3.MoveTowards(
            transform.position, 
            destination, 
            speed * Time.deltaTime
        );
    }

    void OnTriggerEnter(Collider other)
    {
        if (isDead) return;

        if(other.gameObject.CompareTag("Base"))
        {
            if(grove != null)
        {
            grove.AddCoins(coinOnDeath);
            grove.DealDamageToBase(damageOnImpact);
        }
        Destroy(gameObject);
        }  
    }
    public void TakeDamage(float damage)
    {
        if(isDead) return;

        currentHealth -= damage;

        if(healthBar != null)
        {
            healthBar.UpdateHealthBar(Mathf.Max(currentHealth, 0), maxHealth);
        }

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    public void ApplySlow(float slowPercantage, float duration, GameObject effectPrefab)
    {
        if (isSlowed)
        {
            StopCoroutine(nameof(SlowCoroutine));
            if(activeSlowEffect != null) Destroy(activeSlowEffect);
        }
        StartCoroutine(SlowCoroutine(slowPercantage,duration, effectPrefab));
    }

    private IEnumerator SlowCoroutine(float slowPercantage, float duration, GameObject effectPrefab)
    {
        isSlowed = true;

        speed = defaultSpeed * slowPercantage;

        if(effectPrefab != null)
        {
            activeSlowEffect = Instantiate(effectPrefab, transform.position, transform.rotation);
            activeSlowEffect.transform.SetParent(this.transform);
        }
        yield return new WaitForSeconds(duration);

        speed = defaultSpeed;
        isSlowed = false;
        if(activeSlowEffect != null) Destroy(activeSlowEffect);
    }

    private void Die()
    {
        if(isDead) return;

        isDead = true;

        gameObject.tag = "Untagged";
        if(enemyCollider != null) enemyCollider.enabled = false;

        GetComponentInChildren<Canvas>().enabled = false;
        
        if(activeSlowEffect != null) Destroy(activeSlowEffect);

        if(audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        if (animator != null)
        {
            animator.SetTrigger("Die");
        } 
        else if (animation != null)
        {
            animation.CrossFade("Anim_Death");
        }
       
        if(grove != null)
        {
            grove.AddCoins(coinOnDeath);
        }

        Destroy(gameObject, 2f);
    }
}
