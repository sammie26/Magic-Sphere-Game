using UnityEngine;

public class ShardsFalling : MonoBehaviour
{
    private Transform cam;

    [Header("Shard Prefabs")]
    [SerializeField] private GameObject[] shardPrefabs;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private float spawnProbability = .25f;
    [SerializeField] private float minZ = 30f;
    [SerializeField] private float maxZ = 40f;
    [SerializeField] private float minX = -10f;
    [SerializeField] private float maxX = 10f;
    [SerializeField] private float Y = 40f;


    private float timer = 0f;

    void Start()
    {
        cam = Camera.main.transform;

        if (shardPrefabs == null || shardPrefabs.Length < 1)
        {
            throw new System.Exception("Please assign at least one shard prefab!");
        }

    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
            if (Random.value >= spawnProbability) SpawnRandomShard();
            else Debug.Log($"{Time.time} - Shard Didnt Spawn: Lost the RNG");
        }

    }

    void SpawnRandomShard()
    {
        GameObject rockToSpawn = shardPrefabs[Random.Range(0, shardPrefabs.Length)];

        // Calculate spawn position
        Vector3 forward = cam.forward;
        forward.y = 0;
        forward.Normalize();

        Vector3 spawnPosition = transform.position
                             + forward * Random.Range(minZ, maxZ)
                             + cam.right * Random.Range(minX, maxX)
                             + Vector3.up * Y;

        Instantiate(rockToSpawn, spawnPosition, Random.rotation);
    }
}