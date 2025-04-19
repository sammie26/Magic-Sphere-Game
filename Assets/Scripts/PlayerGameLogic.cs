using System;
using Unity.Collections;
using UnityEditor.UIElements;
using UnityEngine;

[RequireComponent(typeof(ShieldLogic))]
[RequireComponent(typeof(PlayerMovement))]
public class PlayerGameLogic : MonoBehaviour
{

    private const string ROCK_TAG = "Rock";
    private const string SHARD_TAG = "ShieldShard";
    private const string PORTAL_TAG = "Portal";

    [SerializeField] private Color winColor = Color.green;
    [SerializeField] private Color loseColor = Color.black;
    [SerializeField] private Transform portal;

    private ShieldLogic shield;
    private Rigidbody rb;
    private PlayerMovement playerMovement;


    private Vector3 winCurrentRotationAxis;
    private float winRotationTimer;
    [SerializeField] private float winChangeRotationInterval = 1.5f;
    [SerializeField] private float winRotationSpeed = 450f;


    public enum State
    {
        Playing,
        Lost,
        Won,
    }
    private State currentState = State.Playing;
    public State CurrentState => currentState;

    public float DistanceToPortal
    {
        get
        {
            // https://docs.unity3d.com/ScriptReference/Vector3.Distance.html
            return Math.Abs(Vector3.Distance(transform.position, portal.position));
        }
    }
    void Start()
    {
        shield = GetComponent<ShieldLogic>();
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();

        // Ignore Collisions Between Rocks and Shards
        // https://docs.unity3d.com/6000.0/Documentation/ScriptReference/LayerMask.NameToLayer.html
        // https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Physics.IgnoreCollision.html
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Rocks"), LayerMask.NameToLayer("Shards"));
    }

    // Update is called once per frame
    void Update()
    {
        winRotationTimer += Time.deltaTime;
        if (currentState == State.Won)
        {
            ApplyWinMovement();
        }

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(PORTAL_TAG))
        {
            Debug.Log("You WIN!");
            HandleWin();
        }
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag(ROCK_TAG))
        {
            HandleRockCollision();
            return;
        }

        if (collision.gameObject.CompareTag(SHARD_TAG))
        {
            HandleShieldShardCollision(collision.gameObject);
            return;
        }
    }

    private void HandleRockCollision()
    {
        shield.TakeDamage();
        if (!shield.isAlive)
        {
            Debug.Log("You Should be DEAD!");
            HandleLose();
        }
    }
    private void HandleShieldShardCollision(GameObject gameObject)
    {
        if (shield.IsFullShields)
        {
            Debug.Log("You Are Full Shields");
            return;
        }

        // Destroy(gameObject);
        shield.RegenerateShield();
    }
    public void HandleLose()
    {
        currentState = State.Lost;
        playerMovement.enabled = false;

        // Remove all RigidBodyForces 
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true; 

        // Color the Core
        GameObject core = transform.Find("Core").gameObject;
        core.GetComponent<Renderer>().material.color = loseColor;

        // Remove Movment Script
        playerMovement.enabled = false;


    }
    void HandleWin()
    {
        currentState = State.Won;

        shield.SetActiveShieldColor(winColor);

        // Disable Scripts
        GetComponent<RocksFalling>().enabled = false;
        GetComponent<ShardsFalling>().enabled = false;
        playerMovement.enabled = false;

        // Freeze Physics Based Movement.
        rb.isKinematic = true;
        rb.useGravity = false;
    }
    void ApplyWinMovement()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.transform.Translate(Vector3.up * Time.deltaTime * 1.5f, Space.World);
        transform.Rotate(winCurrentRotationAxis, winRotationSpeed * Time.deltaTime, Space.World);
        if (winRotationTimer >= winChangeRotationInterval)
        {
            winCurrentRotationAxis = new Vector3(
                UnityEngine.Random.Range(-1f, 1f),
                UnityEngine.Random.Range(-1f, 1f),
                UnityEngine.Random.Range(-1f, 1f)
                ).normalized;
            winRotationTimer = 0f;
        }
    }
}
