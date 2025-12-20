using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("References")]
    [SerializeField] private Transform cameraTransform;

    // Internal Variables
    private Rigidbody rigidBody; // Renamed from 'controller' for clarity
    private PlayerInput playerInput;
    private InputAction moveAction;
    private Vector2 inputVector;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();

        // Auto-find main camera
        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }

        // Setup Input
        moveAction = playerInput.actions["Move"];

        // CONFIGURATION: Ensure Rigidbody settings are correct for a character
        rigidBody.freezeRotation = true; // Prevents character from falling over
        rigidBody.useGravity = true;     // Let Unity handle gravity
        rigidBody.interpolation = RigidbodyInterpolation.Interpolate; // Smoothes movement
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Inputs are read every frame (Update) to prevent missing clicks/taps
        ReadInput();
    }

    private void FixedUpdate()
    {
        // Physics are applied in FixedUpdate
        MovePlayer();
    }

    private void ReadInput()
    {
        if (moveAction != null)
        {
            inputVector = moveAction.ReadValue<Vector2>();
        }
    }

    private void MovePlayer()
    {
        // 1. Calculate Camera Direction
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        // Flatten direction to prevent flying up/down when looking up/down
        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDirection = (camForward * inputVector.y + camRight * inputVector.x).normalized;

        // 2. Apply Velocity (Movement)
        // We preserve the current Y velocity so gravity continues to work
        Vector3 targetVelocity = moveDirection * moveSpeed;
        targetVelocity.y = rigidBody.linearVelocity.y; 

        rigidBody.linearVelocity = targetVelocity;

        // 3. Apply Rotation
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            // using MoveRotation is smoother for Physics objects than transform.rotation
            Quaternion nextRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
            rigidBody.MoveRotation(nextRotation);
        }
    }
}