using UnityEngine;
using UnityEngine.UI; // Needed for Button

public class PauseManager : MonoBehaviour
{
    [Header("Pause Settings")]
    [Tooltip("UI Panel or Canvas to show when game is paused.")]
    public GameObject pauseMenuUI;

    [Tooltip("The Resume Button inside the pause menu.")]
    public Button resumeButton;

    private bool isPaused = false;

    private void Start()
    {
        if (resumeButton != null)
        {
            resumeButton.onClick.AddListener(ResumeGame);
        }
        else
        {
            Debug.LogWarning("Resume Button not assigned in PauseManager!");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);
        }
        isPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
        }
        isPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
