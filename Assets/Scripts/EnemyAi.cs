using System.Collections;
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
    new Animation animation;
    private float defaultSpeed;
    private bool isSlowed;
    private GameObject activeSlowEffect;

    void Start()
    {
        defaultSpeed = speed;
        grove = FindFirstObjectByType<GroveController>();
        gameObject.transform.LookAt(grove.transform);
        target = new Vector3(grove.transform.position.x, gameObject.transform.position.y, grove.transform.position.z);
        currentHealth = maxHealth;
        healthBar.UpdateHealthBar(currentHealth, maxHealth);
        if(GetComponentInChildren<Animator>() != null)
        {
            animator = GetComponentInChildren<Animator>();
        }
        else
        {
            animation = GetComponent<Animation>();
        }
        
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

    public void ApplySlow(float slowPercantage, float duration, GameObject effectPrefab)
    {
        if (isSlowed)
        {
            StopCoroutine("SlowCoroutine");
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
        if(activeSlowEffect != null) Destroy(activeSlowEffect);
        speed = defaultSpeed;
        isSlowed = false;
    }

    private void Die()
    {
        if(isDead) return;

        isDead = true;

        GetComponentInChildren<Canvas>().enabled = false;
        GetComponent<Collider>().enabled = false;
        gameObject.tag = "Untagged";
        if(activeSlowEffect != null) Destroy(activeSlowEffect);

        try
        {
            animator.SetTrigger("Die");
        }
        catch
        {
            Debug.Log("GameObejct does not have a death aniamtion");
        }

        if(animator != null)
        {
            animator.SetTrigger("Die");
        }
        else
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
