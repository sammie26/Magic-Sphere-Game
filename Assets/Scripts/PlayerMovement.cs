using Unity.VisualScripting;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class PlayerMovement : MonoBehaviour
{

    [Header("Important")]
    [SerializeField] private Transform cam;
    [SerializeField] private LayerMask groundLayer;

    [Header("Movement")]
    [SerializeField] private float walkSpeedFactor = 50f;
    [SerializeField] private float maxAngularVelocity = 100f;
    [SerializeField] private int jumpThrust = 50;


    private Rigidbody rb;
    // Input storage
    private float verticalInput, horizontalInput;
    private bool jumpInput;
    private bool isRunning;

    private bool _isGrounded = true;
    private float distToGround; // For Raycast

    // FOR UI
    public enum MovementState
    {
        Resting,
        Climbing
    }
    public enum MovementSpeed
    {
        Fast,
        Slow,
    }
    public MovementState movementState;
    public MovementSpeed movementSpeed => isRunning ? MovementSpeed.Fast : MovementSpeed.Slow;
   


    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Override Player rigidbody.
        rb.maxAngularVelocity = maxAngularVelocity;
        rb.angularDamping = 4f;
        rb.linearDamping = 0.1f;
        rb.mass = 10.0f;

        float colliderRadius = GetComponent<SphereCollider>().radius;
        distToGround = colliderRadius + 0.025f; //Sphere Collider Radius + Artistic Tolerance

        // If Defaults Arent Set.
        if (cam == null)
        {
            cam = Camera.main.transform;
        }

        if (groundLayer == LayerMask.NameToLayer("Default"))
        {
            groundLayer = LayerMask.GetMask("Ground");
        }
    }

    void Update()
    {
        // Read input in Update for better responsiveness
        verticalInput = Input.GetAxis("Vertical");
        horizontalInput = Input.GetAxis("Horizontal");
        jumpInput = Input.GetButton("Jump");
        isRunning = Input.GetButton("Fire3");

        // UI Updates
        if (verticalInput != 0 || horizontalInput != 0) movementState = MovementState.Climbing;
        else movementState = MovementState.Resting;
    }

    void FixedUpdate()
    {
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, distToGround, groundLayer);

        if (_isGrounded) HandleMove(); // Only Apply Torque when Grounded (gameplay choice).
        if (_isGrounded && jumpInput == true) HandleJump();
    }
    private void HandleMove()
    {
        Vector3 moveDirection = (cam.right * verticalInput) + (horizontalInput * -cam.forward);
        float currentSpeedFactor = isRunning ? walkSpeedFactor * 2 : walkSpeedFactor;
        Vector3 torqueVector = currentSpeedFactor * moveDirection.normalized;
        // https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Rigidbody.AddTorque.html
        rb.AddTorque(torqueVector, ForceMode.Force);
    }

    private void HandleJump()
    {
        int jumpForceToUse = isRunning ? jumpThrust * 2 : jumpThrust;
        Debug.Log($"Jumpforce to Use: {jumpForceToUse}");
        rb.AddForce(jumpForceToUse * Vector3.up, ForceMode.Impulse);
    }

}
