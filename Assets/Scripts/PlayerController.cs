using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))] // Forces PlayerInput to exist
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    [SerializeField] private float gravity = -9.81f;

    [Header("References")]
    [SerializeField] private Transform cameraTransform;

    // Internal Variables
    private CharacterController controller;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private Vector2 inputVector;
    private Vector3 velocity; // For gravity

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();

        // Auto-find main camera
        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }

        // SETUP INPUT
        // IMPORTANT: The string "Move" must match the Action Name in your Input Action Asset exactly!
        moveAction = playerInput.actions["Move"];
    }

    private void Start()
    {
        // --- CURSOR LOCKING ---
        // This hides the cursor and locks it to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        ReadInput();
        MovePlayer();
        ApplyGravity();
    }

    private void ReadInput()
    {
        if (moveAction != null)
        {
            inputVector = moveAction.ReadValue<Vector2>();
            
            // UNCOMMENT THE LINE BELOW TO DEBUG
            // Debug.Log("Input detected: " + inputVector); 
        }
    }

    private void MovePlayer()
    {
        // 1. If no input, stop processing horizontal movement
        if (inputVector.magnitude < 0.1f) return;

        // 2. Camera Direction Math
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 targetDirection = (camForward * inputVector.y + camRight * inputVector.x).normalized;

        // 3. Move
        controller.Move(targetDirection * moveSpeed * Time.deltaTime);

        // 4. Rotate
        if (targetDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    private void ApplyGravity()
    {
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; 
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}