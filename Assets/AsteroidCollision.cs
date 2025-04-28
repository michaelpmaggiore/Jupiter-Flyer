using UnityEngine;

public class AsteroidCollision : MonoBehaviour
{
    [Tooltip("Optional: Explosion effect prefab to spawn on impact.")]
    public GameObject explosionPrefab;

    public AudioSource audioSource;

    public AudioClip asteroidExplosionSound;

    // At the start of the game..
    void Start()
    {

        // Ensure the AudioSource is set up correctly.
        if (audioSource != null && asteroidExplosionSound != null)
        {
            audioSource.clip = asteroidExplosionSound;
            audioSource.loop = false;
        }
        else
        {
            Debug.LogWarning("AudioSource or asteroidExplosionSound AudioClip is missing!");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if we hit an object with the AsteroidMarker script
        AsteroidMarker asteroid = collision.collider.GetComponent<AsteroidMarker>();

        if (asteroid != null)
        {
            // We hit an asteroid!
            audioSource.Play();

            // Spawn explosion effect if assigned
            if (explosionPrefab != null)
            {
                // Spawn explosion
                GameObject explosion = Instantiate(explosionPrefab, collision.contacts[0].point, Quaternion.identity);

                // 1. Scale it bigger
                explosion.transform.localScale *= 10;

                // 2. Change color (assuming it has a ParticleSystem)
                ParticleSystem[] allParticles = explosion.GetComponentsInChildren<ParticleSystem>();
                foreach (ParticleSystem ps in allParticles)
                {
                    var main = ps.main;
                    main.startColor = Color.red;
                }

                Destroy(explosion, 2.0f);

            }

            // Destroy the asteroid
            Destroy(collision.gameObject);

            // Destroy the bullet
            Destroy(gameObject);
        }
        else
        {
            // Not an asteroid: just destroy the bullet
            Destroy(gameObject);
        }
    }
}
