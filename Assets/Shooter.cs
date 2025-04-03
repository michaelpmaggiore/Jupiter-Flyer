using UnityEngine;

public class Shooter : MonoBehaviour
{
    public GameObject projectilePrefab; // Assign in the Inspector
    public Transform spawnPoint; // Assign a spawn point (or use ball position)
    public float launchSpeed; // Adjust force for better effect
    public Transform camera;
    public Rigidbody playerRigidbody;
    public float lifetime = 5f; // Lifetime of the projectile

    [Tooltip("AudioSource component to play the rocket sound.")]
    public AudioSource audioSource;
    
    [Tooltip("Rocket sound effect AudioClip.")]
    public AudioClip rocketSound;

    // At the start of the game..
	void Start ()
	{

		if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
        
        // Ensure the AudioSource is set up correctly.
        if (audioSource != null && rocketSound != null)
        {
            audioSource.clip = rocketSound;
            audioSource.loop = false;
        }
        else
        {
            Debug.LogWarning("AudioSource or RocketSound AudioClip is missing!");
        }
	}

    private void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Right-click to shoot
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        if (projectilePrefab == null)
        {
            Debug.LogError("Projectile Prefab is not assigned!");
            return;
        }

        audioSource.loop = false;  // Ensure looping is enabled.
        audioSource.Play();

        // Spawn the projectile at the spawn position (or use the ball's position)
        GameObject projectile = Instantiate(projectilePrefab, spawnPoint.position, Quaternion.identity);
        Destroy(projectile, lifetime);

        // Add Rigidbody to the projectile
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            //rb.AddForce(Vector3.up * launchForce, ForceMode.Impulse); // Shoot into space
            Vector3 shootDirection = camera.forward; // Shoot in the direction the camera is facing
            rb.linearVelocity = (shootDirection * launchSpeed); // Shoot with a specific speed
        }
        else
        {
            Debug.LogError("Projectile does not have a Rigidbody!");
        }
    }
}
