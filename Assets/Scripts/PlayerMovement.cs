using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    [Header("References")]
    public CharacterController controller;
    public Transform cameraTransform;

    [Header("Movement")]
    public float moveSpeed = 6f;
    public float gravity = -9.81f;
    public float jumpHeight = 2f;

    private Vector2 moveInput;
    private bool jumpPressed;
    private float yVelocity;

    private void Update()
    {
        MovePlayer();
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        jumpPressed = value.isPressed;
    }

    private void MovePlayer()
    {
        // Kameranın yönüne göre forward/right elde et
        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;

        // Y eksenini yok et (eğik kameralarda yere gömülmemek için)
        camForward.y = 0;
        camRight.y = 0;

        camForward.Normalize();
        camRight.Normalize();

        // Input’u kamera yönüne göre dönüştür
        Vector3 moveDir = (camForward * moveInput.y + camRight * moveInput.x);

        // Yerçekimi
        if (controller.isGrounded)
        {
            yVelocity = -1f;
            if (jumpPressed)
            {
                yVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }
        else
        {
            yVelocity += gravity * Time.deltaTime;
        }

        // Son hareket
        Vector3 velocity = moveDir * moveSpeed + Vector3.up * yVelocity;
        controller.Move(velocity * Time.deltaTime);
    }
}
