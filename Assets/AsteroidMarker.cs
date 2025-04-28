using UnityEngine;

public class AsteroidMarker : MonoBehaviour
{
    [Tooltip("Damage to deal to the space station on collision.")]
    public int damageAmount = 10;

    [Tooltip("Prefab to spawn when asteroid explodes.")]
    public GameObject explosionPrefab;

    [Tooltip("AudioSource used to play asteroid explosion sound.")]
    public AudioSource audioSource;

    [Tooltip("Explosion sound clip.")]
    public AudioClip asteroidExplosionSound;

    [Tooltip("How long the explosion effect should last before being destroyed.")]
    public float explosionLifetime = 3f;

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
        // Check if the thing we hit has a SpaceStationMarker
        SpaceStationMarker stationPart = collision.collider.GetComponent<SpaceStationMarker>();

        if (stationPart != null)
        {
            // We hit the space station!

            // Subtract health globally
            if (SpaceStationHealth.Instance != null)
            {
                SpaceStationHealth.Instance.SetHealth(SpaceStationHealth.Instance.GetHealth() - damageAmount);
            }

            audioSource.Play();

            // Spawn explosion
            if (explosionPrefab != null)
            {
                GameObject explosion = Instantiate(explosionPrefab, collision.contacts[0].point, Quaternion.identity);
                explosion.transform.localScale *= 50;
                ParticleSystem[] allParticles = explosion.GetComponentsInChildren<ParticleSystem>();
                foreach (ParticleSystem ps in allParticles)
                {
                    var main = ps.main;
                    main.startColor = Color.blue;
                }

                Destroy(explosion, explosionLifetime);
            }

            // Destroy asteroid after impact
            Destroy(gameObject);
        }
    }

}
