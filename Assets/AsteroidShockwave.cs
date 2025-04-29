using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;


public class AsteroidShockwave : MonoBehaviour
{

    public static AsteroidShockwave Instance; // Singleton instance

    public int asteroidsNeeded = 20; // Number of asteroids needed to trigger the shockwave
    public float shockwaveRadius = 500f; // Radius of the shockwave effect
    public Slider slider; // slider which displays the progress towards a shockwave
    public float explosionLifetime = 3f; // Lifetime of the asteroid explosion effects
    public float timeSlowDuration = 1f; // Duration of the time slow effect
    public float timeSlowScale = 0.2f; // Time scale during the slow effect

    public Transform playerTransform; // Reference to the player transform

    public Text useShockwaveText; // Text to display the shockwave use message

    public AudioSource shockwaveAudioSource;
    public AudioClip shockwaveSound;
    public GameObject asteroidExplosionPrefab;

    private int currentAsteroids = 0; // Current number of asteroids collected

    void Awake()
    {
        // Ensure that only one instance of this class exists
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentAsteroids = 0;
        if (slider != null)
        {
            slider.maxValue = asteroidsNeeded;
            slider.value = 0;
        }
        else
        {
            Debug.LogWarning("Slider is not assigned!");
        }
        // Ensure the AudioSource is set up correctly.
        if (shockwaveAudioSource != null && shockwaveSound != null)
        {
            shockwaveAudioSource.clip = shockwaveSound;
            shockwaveAudioSource.loop = false;
        }
        else
        {
            Debug.LogWarning("AudioSource or ShockwaveSound AudioClip is missing!");
        }
    }

    public void AddAsteroid()
    {
        currentAsteroids++;
        UpdateSlider();
        //Debug.Log("Asteroid added. Current Asteroids: " + currentAsteroids);

    }

    private void UpdateSlider()
    {
        if (slider != null)
        {
            slider.value = (float)currentAsteroids;
        }
        else
        {
            Debug.LogWarning("Slider is not assigned!");
        }
    }

    private void TriggerShockwave()
    {
        // Logic to trigger the shockwave effect
        Debug.Log("Shockwave triggered!");
        shockwaveAudioSource.Play();

        AsteroidMarker[] markers = FindObjectsByType<AsteroidMarker>(FindObjectsSortMode.None);

        


        foreach (AsteroidMarker marker in markers)
        {
            if (Vector3.Distance(marker.transform.position, playerTransform.position) <= shockwaveRadius)
            {
                GameObject explosion = Instantiate(asteroidExplosionPrefab, marker.transform.position, Quaternion.identity);
                explosion.transform.localScale *= 10;
                Destroy(explosion, explosionLifetime);
                Destroy(marker.gameObject);
            }
        }



        // Reset the current asteroids count
        currentAsteroids = 0;
        UpdateSlider();

        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentAsteroids >= asteroidsNeeded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                TriggerShockwave();
            }
            useShockwaveText.gameObject.SetActive(true);
        }
        else
        {
            useShockwaveText.gameObject.SetActive(false);
        }
    }
}
