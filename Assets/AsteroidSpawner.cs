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

    [Tooltip("How often to spawn asteroids (seconds).")]
    public float spawnInterval = 2f;

    [Tooltip("How many asteroids per spawn burst.")]
    public int asteroidsPerSpawn = 1;

    [Header("Asteroid Movement Settings")]
    [Tooltip("Speed of the spawned asteroids.")]
    public float asteroidSpeed = 10f;

    [Tooltip("Amount of random spread added to asteroid trajectory (degrees).")]
    public float randomSpreadAngle = 10f;

    private float spawnTimer = 0f;

    private void Update()
    {
        if (asteroidPrefab == null || target == null)
            return;

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            for (int i = 0; i < asteroidsPerSpawn; i++)
            {
                SpawnSingleAsteroid();
            }
            spawnTimer = 0f;
        }
    }

    private void SpawnSingleAsteroid()
    {
        // Choose a random spawn position on a sphere around the target
        Vector3 randomOffset = Random.onUnitSphere * spawnRadius;
        Vector3 spawnPosition = target.position + randomOffset;

        // Random orientation
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
