using UnityEngine;

public class AsteroidSpawner : MonoBehaviour
{
    [Header("Spawning Settings")]
    [Tooltip("Prefab of the asteroid to spawn.")]
    public GameObject asteroidPrefab;

    [Tooltip("Target the asteroids should home toward.")]
    public Transform target;

    [Tooltip("How far from the target asteroids spawn.")]
    public float spawnRadius = 100f;

    [Tooltip("Initial time between spawns (seconds).")]
    public float initialSpawnInterval = 2f;

    [Tooltip("Minimum spawn interval (can't go faster than this).")]
    public float minSpawnInterval = 0.2f;

    [Tooltip("How fast the spawn interval decreases (seconds per second).")]
    public float spawnAccelerationRate = 0.05f; // seconds decrease per second

    [Tooltip("How many asteroids per spawn burst.")]
    public int asteroidsPerSpawn = 1;

    [Header("Asteroid Movement Settings")]
    [Tooltip("Speed of the spawned asteroids.")]
    public float asteroidSpeed = 10f;

    [Tooltip("Amount of random spread added to asteroid trajectory (degrees).")]
    public float randomSpreadAngle = 10f;

    private float spawnTimer = 0f;
    private float currentSpawnInterval;

    private void Start()
    {
        currentSpawnInterval = initialSpawnInterval;
    }

    private void Update()
    {
        if (asteroidPrefab == null || target == null)
            return;

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= currentSpawnInterval)
        {
            for (int i = 0; i < asteroidsPerSpawn; i++)
            {
                SpawnSingleAsteroid();
            }
            spawnTimer = 0f;
        }

        // Gradually decrease spawn interval over time
        currentSpawnInterval -= spawnAccelerationRate * Time.deltaTime;
        currentSpawnInterval = Mathf.Max(currentSpawnInterval, minSpawnInterval);
    }

    private void SpawnSingleAsteroid()
    {
        Vector3 randomOffset = Random.onUnitSphere * spawnRadius;
        Vector3 spawnPosition = target.position + randomOffset;
        Debug.Log("Spawning asteroid. Spawn Interval = " + currentSpawnInterval);

        Quaternion randomRotation = Random.rotation;

        GameObject newAsteroid = Instantiate(asteroidPrefab, spawnPosition, randomRotation);

        Renderer renderer = newAsteroid.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.enabled = true;
        }

        Rigidbody rb = newAsteroid.GetComponent<Rigidbody>();
        if (rb != null)
        {
            Vector3 directionToTarget = (target.position - spawnPosition).normalized;
            Vector3 finalDirection = ApplyRandomSpread(directionToTarget, randomSpreadAngle);

            rb.linearVelocity = finalDirection * asteroidSpeed;
        }
        else
        {
            Debug.LogWarning("Spawned asteroid does not have a Rigidbody!");
        }
    }

    private Vector3 ApplyRandomSpread(Vector3 direction, float maxAngle)
    {
        return Quaternion.AngleAxis(Random.Range(-maxAngle, maxAngle), Random.insideUnitSphere) * direction;
    }
}
