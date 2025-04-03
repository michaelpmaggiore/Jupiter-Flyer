using UnityEngine;

public class TimeSlowController : MonoBehaviour
{
    [Tooltip("Time scale when slowing down time.")]
    public float slowTimeScale = 0.2f;
    
    [Tooltip("Normal time scale.")]
    public float normalTimeScale = 1.0f;
    
    [Tooltip("Speed at which time scale transitions.")]
    public float transitionSpeed = 5.0f;

    private float targetTimeScale;
    private bool slowModeActive = false;
    private bool spaceReleased = true;

    private void Start()
    {
        targetTimeScale = normalTimeScale;
    }

    private void Update()
    {
        // If left-click is pressed, reset to normal speed immediately.
        if (Input.GetMouseButtonDown(0))
        {
            slowModeActive = false;
        }

        // Check for space key input:
        // Only engage slow mode if space is freshly pressed (i.e., after being released).
        if (Input.GetKeyDown(KeyCode.D) && spaceReleased)
        {
            if (slowModeActive)
            {
                slowModeActive = false;
            }
            else
            {
                slowModeActive = true;
            }
            spaceReleased = false;
        }

        // When space is released, allow it to trigger slow mode again.
        if (Input.GetKeyUp(KeyCode.D))
        {
            spaceReleased = true;
        }

        // Set the target time scale based on slow mode status.
        targetTimeScale = slowModeActive ? slowTimeScale : normalTimeScale;

        // Smoothly transition the time scale for a gradual effect.
        Time.timeScale = Mathf.Lerp(Time.timeScale, targetTimeScale, Time.unscaledDeltaTime * transitionSpeed);
        
        // Adjust fixedDeltaTime to keep physics stable.
        Time.fixedDeltaTime = 0.02f * Time.timeScale;
    }
}
