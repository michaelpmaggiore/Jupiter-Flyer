using UnityEngine;

public class TimeSlowController : MonoBehaviour
{
    [Tooltip("Time scale when slowing down time.")]
    public float slowTimeScale = 0.2f;
    
    [Tooltip("Normal time scale.")]
    public float normalTimeScale = 1.0f;
    
    [Tooltip("Speed at which time scale transitions.")]
    public float transitionSpeed = 5.0f;

    [Tooltip("Sound effect to play when slowing down.")]
    public AudioClip slowdownSound;
    
    [Tooltip("Sound effect to play when speeding up.")]
    public AudioClip speedupSound;


    public AudioSource audioSource;
    private float targetTimeScale;
    private bool slowModeActive = false;
    private bool keyReleased = true;

    private void Start()
    {
        targetTimeScale = normalTimeScale;
        //audioSource = GetComponent<AudioSource>();
        // if (audioSource == null)
        // {
        //     Debug.LogWarning("AudioSource not found on this GameObject. Please add one to play sound effects.");
        // }
        audioSource.pitch = 2.0f;
    }

    private void Update()
    {
        // If left-click is pressed, reset to normal speed immediately.
        if (Input.GetMouseButtonDown(0))
        {
            if (slowModeActive)
            {
                slowModeActive = false;
                PlaySpeedupSound();
            }
        }

        // Toggle slow mode with the D key (only if the key was released previously).
        if (Input.GetKeyDown(KeyCode.D) && keyReleased)
        {
            if (slowModeActive)
            {
                slowModeActive = false;
                PlaySpeedupSound();
            }
            else
            {
                slowModeActive = true;
                PlaySlowdownSound();
            }
            keyReleased = false;
        }

        // Reset the keyReleased flag when D is released.
        if (Input.GetKeyUp(KeyCode.D))
        {
            keyReleased = true;
        }

        // Set the target time scale based on slow mode status.
        targetTimeScale = slowModeActive ? slowTimeScale : normalTimeScale;
        
        // Smoothly transition the time scale.
        Time.timeScale = Mathf.Lerp(Time.timeScale, targetTimeScale, Time.unscaledDeltaTime * transitionSpeed);
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }

    private void PlaySlowdownSound()
    {
        if (audioSource != null && slowdownSound != null)
        {
            audioSource.PlayOneShot(slowdownSound);
        }
    }

    private void PlaySpeedupSound()
    {
        if (audioSource != null && speedupSound != null)
        {
            audioSource.PlayOneShot(speedupSound);
        }
    }
}
