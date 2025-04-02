using UnityEngine;

public class BulletDestroyer : MonoBehaviour
{
    [Tooltip("Time in seconds after which the bullet is destroyed.")]
    public float lifetime = 5f;

    [Tooltip("Explosion effect prefab to spawn when the bullet is destroyed.")]
    public GameObject explosionEffect;

    private bool hasExploded = false;

    private void Start()
    {
        // Schedule the Explode method to be called after 'lifetime' seconds.
        Invoke("Explode", lifetime);
    }

    private void Explode()
    {
        if (hasExploded)
            return;
        hasExploded = true;

        // Instantiate the explosion effect if assigned.
        if (explosionEffect != null)
        {
            GameObject explosion = Instantiate(explosionEffect, transform.position, transform.rotation);
            // Clean up the explosion effect after a short delay (if needed).
            Destroy(explosion, 2f);
        }

        // Destroy the bullet.
        Destroy(gameObject);
    }
}
