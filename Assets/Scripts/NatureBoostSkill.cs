using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class NatureBoostSkill : MonoBehaviour
{
    [Header("Skill Settings")]
    [SerializeField] private float boostAmount = 1.3f;
    [SerializeField] private float boostDuration = 5f;
    [SerializeField] private float cooldownDuration = 20f;
    [SerializeField] private string inputActionName = "NatureBoost";
    [SerializeField] private GameObject boostParticlePrefab;

    [Header("Audio")]
    [SerializeField] private AudioClip boostSound;

    [Header("UI Reference")]
    [SerializeField] private Image cooldownOverlay;

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

        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");
        
        if (towers.Length == 0)
        {
            Debug.Log("No towers found.");
            return; 
        }

        ActivateSkill(towers);
        
        // Start Cooldown
        currentCooldownTimer = cooldownDuration;

        if(audioSource != null && boostSound != null)
        {
            audioSource.PlayOneShot(boostSound);
        }
        
        // Immediately set UI to full
        if (cooldownOverlay != null)
        {
            cooldownOverlay.fillAmount = 1f;
        }
    }

    private void ActivateSkill(GameObject[] towers)
    {
        Debug.Log("Nature boost Activated!");
        foreach (GameObject tower in towers)
        {
            TowerController towerController = tower.GetComponent<TowerController>();
            if (towerController != null)
            {
                towerController.ApplyBoost(boostAmount, boostDuration, boostParticlePrefab);
            }
        }
    }
}
