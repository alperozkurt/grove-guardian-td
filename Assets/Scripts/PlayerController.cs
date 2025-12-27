using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 10f;
    [SerializeField] private float sprintSpeed = 18f;
    [SerializeField] private float rotationSpeed = 10f;

    [Header("References")]
    [SerializeField] private Transform cameraTransform;

    // Internal Variables
    private Rigidbody rigidBody;
    private PlayerInput playerInput;
    private Animator animator;
    
    private InputAction moveAction;
    private InputAction sprintAction;
    
    private Vector2 inputVector;
    private bool isSprinting;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();

        if (cameraTransform == null && Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }

        // Setup Input
        // NOTE: Ensure you define an Action named "Sprint" in your Input Action Asset!
        moveAction = playerInput.actions["Move"];
        sprintAction = playerInput.actions["Sprint"];

        rigidBody.freezeRotation = true;
        rigidBody.useGravity = true;
        rigidBody.interpolation = RigidbodyInterpolation.Interpolate;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        ReadInput();
        UpdateAnimations(); // New: Update animations every frame
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void ReadInput()
    {
        if (moveAction != null)
        {
            inputVector = moveAction.ReadValue<Vector2>();
        }

        if (sprintAction != null)
        {
            // Read if the button is currently being held down
            isSprinting = sprintAction.IsPressed();
        }
    }

    private void MovePlayer()
    {
        // 1. Calculate Camera Direction
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        camForward.y = 0;
        camRight.y = 0;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDirection = (camForward * inputVector.y + camRight * inputVector.x).normalized;

        // 2. Determine Speed (Walk vs Sprint)
        // Only sprint if moving forward/sideways and holding shift
        float targetSpeed = (isSprinting && inputVector != Vector2.zero) ? sprintSpeed : walkSpeed;

        // 3. Apply Velocity
        Vector3 targetVelocity = moveDirection * targetSpeed;
        targetVelocity.y = rigidBody.linearVelocity.y; // Keep existing gravity

        rigidBody.linearVelocity = targetVelocity;

        // 4. Apply Rotation
        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            Quaternion nextRotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
            rigidBody.MoveRotation(nextRotation);
        }
    }

    private void UpdateAnimations()
    {
        Vector3 horizontalVelocity = rigidBody.linearVelocity;
        horizontalVelocity.y = 0;
        
        float speed = horizontalVelocity.magnitude;

        animator.SetFloat("Speed", speed, 0.1f, Time.deltaTime);
    }

    public void PerformBlizzard() => animator.SetTrigger("Blizzard");
    public void PerformNatureBoost() => animator.SetTrigger("NatureBoost");
}