using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;      // only if you want a UI Text
using System.Collections;
using TMPro;

[RequireComponent(typeof(Renderer), typeof(Collider), typeof(Rigidbody))]
public class PlayerDeathAndRespawn : MonoBehaviour
{
    [Header("Respawn Settings")]
    public Transform respawnPoint;       // assign your respawn empty here
    public float respawnDelay = 1f;      // seconds to wait before respawn

    [Header("Death Effect")]
    public GameObject explosionPrefab;   // your explosion prefab

    [Header("Lives Settings")]
    public int maxLives = 3;             // total lives at start
    public TextMeshProUGUI livesText;               // (optional) drag in a UI Text to show lives
    public TextMeshProUGUI textObject; // Drag your UI Text object here

    private int currentLives;
    private Vector3 startPosition;
    private Quaternion startRotation;

    private void Start()
    {
        // 1) Initialize lives
        currentLives = maxLives;
        UpdateLivesUI();

        // 2) Cache spawn pos/rot
        if (respawnPoint == null)
        {
            startPosition = transform.position;
            startRotation = transform.rotation;
        }
        else
        {
            startPosition = respawnPoint.position;
            startRotation = respawnPoint.rotation;
        }
    }

    private void UpdateLivesUI()
    {
        if (livesText != null)
            livesText.text = $"Lives: {currentLives}";
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Building"))
            StartCoroutine(ExplodeAndRespawn());
    }

    private IEnumerator ExplodeAndRespawn()
    {
        // --- 1) Explosion VFX/SFX
        if (explosionPrefab != null)
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        // --- 2) Disable player visuals & physics
        var rend = GetComponent<Renderer>();
        if (rend != null) rend.enabled = false;
        var col = GetComponent<Collider>();
        if (col != null) col.enabled = false;
        var rb = GetComponent<Rigidbody>();
        if (rb != null) rb.isKinematic = true;

        // --- 3) Decrement lives & update UI
        currentLives--;
        UpdateLivesUI();

        // --- 4) Check for Game Over
        if (currentLives <= 0)
        {
            textObject.gameObject.SetActive(true);
            textObject.text = "You lose! Game restarting...";
            // e.g. reload the scene after a short pause:
            yield return new WaitForSeconds(respawnDelay);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            yield break;
        }

        // --- 5) Wait then respawn
        yield return new WaitForSeconds(respawnDelay);
        transform.position = startPosition;
        transform.rotation = startRotation;

        // --- 6) Re‑enable player
        if (rend != null) rend.enabled = true;
        if (col != null) col.enabled = true;
        if (rb != null) rb.isKinematic = false;
    }
}