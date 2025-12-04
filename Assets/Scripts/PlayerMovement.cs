using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] private InputAction movement;
    [SerializeField] private InputAction look;
    [SerializeField] private InputAction jump;

    [Header("Movement Settings")]
    [SerializeField] private float jumpPower = 1.5f;
    [SerializeField] private float movemenetSpeed = 5f;
    private float gravity = -9.81f;

}
