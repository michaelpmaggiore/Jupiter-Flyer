
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SpaceStationHealth : MonoBehaviour
{
    public static SpaceStationHealth Instance;

    [Tooltip("Total max health of the space station.")]
    public int maxHealth = 100;

    [Tooltip("Current health of the space station.")]
    public int currentHealth;

    [Tooltip("Reference to the UI Slider showing health.")]
    public Slider healthBarSlider;

    [Header("Game Over UI Elements")]
    public GameObject gameOverPanel;
    public Text gameOverMessageText;
    public Text survivalTimeText;
    public Button playAgainButton;
    public Button mainMenuButton;

    private float survivalTimer = 0f;
    private bool isDead = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        currentHealth = maxHealth;

        if (healthBarSlider != null)
        {
            healthBarSlider.maxValue = maxHealth;
            healthBarSlider.value = currentHealth;
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (healthBarSlider != null)
        {
            healthBarSlider.value = currentHealth;
        }

        if (!isDead)
        {
            survivalTimer += Time.deltaTime;
        }
    }

    public void SetHealth(int value)
    {
        if (isDead)
            return;

        currentHealth = Mathf.Clamp(value, 0, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public int GetHealth()
    {
        return currentHealth;
    }

    private void Die()
    {
        isDead = true;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);

            if (gameOverMessageText != null)
            {
                gameOverMessageText.text = "You did not protect the Space Station!";
            }

            if (survivalTimeText != null)
            {
                survivalTimeText.text = $"You lasted {Mathf.FloorToInt(survivalTimer)} seconds.";
            }
        }

        if (playAgainButton != null)
        {
            playAgainButton.onClick.RemoveAllListeners();
            playAgainButton.onClick.AddListener(PlayAgain);
        }

        if (mainMenuButton != null)
        {
            mainMenuButton.onClick.RemoveAllListeners();
            mainMenuButton.onClick.AddListener(ReturnToMainMenu);
        }

        // Optional: Freeze game
        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void PlayAgain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu"); // Make sure you have a scene named "MainMenu"
    }
}





//using UnityEngine;
//using UnityEngine.UI;

//public class SpaceStationHealth : MonoBehaviour
//{
//    public static SpaceStationHealth Instance; // Singleton global access

//    [Tooltip("Total max health of the space station.")]
//    public int maxHealth = 100;

//    [Tooltip("Current health of the space station.")]
//    private int currentHealth;

//    [Tooltip("Reference to the UI Slider showing health.")]
//    public Slider healthBarSlider;

//    private void Awake()
//    {
//        if (Instance != null && Instance != this)
//        {
//            Destroy(gameObject);
//            return;
//        }
//        Instance = this;
//    }

//    private void Start()
//    {
//        currentHealth = maxHealth;

//        if (healthBarSlider != null)
//        {
//            healthBarSlider.maxValue = maxHealth;
//            healthBarSlider.value = currentHealth;
//        }
//    }

//    private void Update()
//    {
//        if (healthBarSlider != null)
//        {
//            healthBarSlider.value = currentHealth;
//        }
//    }

//    public void SetHealth(int value)
//    {
//        currentHealth = Mathf.Clamp(value, 0, maxHealth);
//    }

//    public int GetHealth()
//    {
//        return currentHealth;
//    }
//}
