using UnityEngine;

public class RocksFalling : MonoBehaviour
{
    private Transform cam;

    [Header("Rock Prefabs")]
    [SerializeField] private GameObject[] rockPrefabs;

    [Header("Spawn Settings")]
    [SerializeField] private float initialDelay = 5f;
    [SerializeField] private float minSpawnInterval = 0.1f;
    [SerializeField] private float maxSpawnInterval = 0.5f;
    [SerializeField] private float minZ = 30f;
    [SerializeField] private float maxZ = 40f;
    [SerializeField] private float minX = -10f;
    [SerializeField] private float maxX = 10f;
    [SerializeField] private float Y = 40f;
    [SerializeField] private float minTorque = 5f;
    [SerializeField] private float maxTorque = 20f;

    private float timer = 0f;
    private bool hasStartedSpawning = false;
    private float nextSpawnInterval;

    void Start()
    {
        cam = Camera.main.transform;

        if (rockPrefabs == null || rockPrefabs.Length < 1) // Changed to allow any number of prefabs
        {
            throw new System.Exception("Please assign at least one shard prefab!");
        }

        nextSpawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (!hasStartedSpawning && timer >= initialDelay)
        {
            hasStartedSpawning = true;
            timer = 0f;
            SpawnRandomRock();
        }
        else if (hasStartedSpawning && timer >= nextSpawnInterval)
        {
            timer = 0f;
            nextSpawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
            SpawnRandomRock();
        }
    }

    void SpawnRandomRock()
    {
        GameObject rockToSpawn = rockPrefabs[Random.Range(0, rockPrefabs.Length)];

        // Calculate spawn position
        Vector3 forward = cam.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 spawnPosition = transform.position
                             + forward * Random.Range(minZ, maxZ)
                             + cam.right * Random.Range(minX, maxX)
                             + Vector3.up * Y;

        // Instantiate with physics
        GameObject newRock = Instantiate(rockToSpawn, spawnPosition, Random.rotation);
        Vector3 randomTorque = new Vector3(
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f),
            Random.Range(-1f, 1f)
        ).normalized * Random.Range(minTorque, maxTorque);
        newRock.GetComponent<Rigidbody>().AddTorque(randomTorque, ForceMode.Impulse);
    }
}