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
    public AudioClip slowSound;

    public AudioSource audioSource;

    private float targetTimeScale;
    private bool slowModeActive = false;

    private void Start()
    {
        targetTimeScale = normalTimeScale;

        if (audioSource != null && slowSound != null)
        {
            audioSource.clip = slowSound;
            audioSource.loop = true;
            audioSource.volume = 0.25f;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning("AudioSource or slowSound AudioClip is missing!");
        }
    }

    private void Update()
    {
        // Always match audio pitch to time scale
        if (audioSource != null)
        {
            audioSource.pitch = Time.timeScale;
        }

        // Toggle slow mode when pressing Space
        if (Input.GetKeyDown(KeyCode.Space))
        {
            slowModeActive = !slowModeActive;
        }

        // Immediately cancel slow mode if left-click
        if (Input.GetMouseButtonDown(0))
        {
            slowModeActive = false;
        }

        // Set the target time scale
        targetTimeScale = slowModeActive ? slowTimeScale : normalTimeScale;

        // Smoothly transition the time scale
        Time.timeScale = Mathf.Lerp(Time.timeScale, targetTimeScale, Time.unscaledDeltaTime * transitionSpeed);
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }
}

