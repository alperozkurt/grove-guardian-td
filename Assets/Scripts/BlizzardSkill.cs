using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(PlayerInput))]
public class BlizzardSkill : MonoBehaviour
{
    [Header("Skill Settings")]
    [SerializeField] private float damageAmount = 20f;
    [SerializeField] private float cooldownDuration = 15f;
    [SerializeField] private string inputActionName = "Blizzard";

    [Header("Slow Effect")]
    [SerializeField] private float slowPercantage = 0.6f;
    [SerializeField] private float slowDuration = 4f;
    [SerializeField] private GameObject freezeParticlePrefab;

    [Header("Audio")]
    [SerializeField] private AudioClip blizzardSound;

    [Header("UI Reference")]
    [SerializeField] private Image cooldownOverlay;

    // Internal Variables
    private PlayerInput playerInput;
    private InputAction skillAction;
    private AudioSource audioSource;
    private float currentCooldownTimer = 0f;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        skillAction = playerInput.actions[inputActionName];
        audioSource = GetComponent<AudioSource>();

        if(cooldownOverlay != null)
        {
            cooldownOverlay.fillAmount = 0f;
        }
    }

    private void Update()
    {
        // 1. Handle Cooldown Timer
        if (currentCooldownTimer > 0)
        {
            currentCooldownTimer -= Time.deltaTime;

            // Update the UI Fill Amount
            if (cooldownOverlay != null)
            {
                // Calculate percentage (0.0 to 1.0)
                cooldownOverlay.fillAmount = currentCooldownTimer / cooldownDuration;
            }
        }
        else
        {
            // Ensure it stays at 0 when finished
            if (cooldownOverlay != null) 
            {
                cooldownOverlay.fillAmount = 0f;
            }
        }

        // 2. Read Input
        if (skillAction != null && skillAction.WasPressedThisFrame())
        {
            AttemptToCast();
        }
    }

    private void AttemptToCast()
    {
        if (currentCooldownTimer > 0) return;

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        
        if (enemies.Length == 0)
        {
            return; 
        }

        ActivateSkill(enemies);
        
        // Start Cooldown
        currentCooldownTimer = cooldownDuration;

        if(audioSource != null && blizzardSound != null)
        {
            audioSource.PlayOneShot(blizzardSound);
        }
        
        // Immediately set UI to full
        if (cooldownOverlay != null)
        {
            cooldownOverlay.fillAmount = 1f;
        }
    }

    private void ActivateSkill(GameObject[] enemies)
    {
        GetComponent<PlayerController>().PerformBlizzard();
        foreach (GameObject enemy in enemies)
        {
            EnemyAi enemyAi = enemy.GetComponent<EnemyAi>();
            BossAi bossAi = enemy.GetComponent<BossAi>();
            if (enemyAi != null)
            {
                enemyAi.TakeDamage(damageAmount);
                enemyAi.ApplySlow(slowPercantage, slowDuration, freezeParticlePrefab);
            }
            else
            {
                bossAi.TakeDamage(damageAmount);
                bossAi.ApplySlow(slowPercantage, slowDuration, freezeParticlePrefab);
            }
        }
    }
}