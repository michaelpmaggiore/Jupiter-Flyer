using UnityEngine;
using UnityEngine.UI; // Legacy Text, not TextMeshPro

public class ActiveTurretCount : MonoBehaviour
{
    [Header("Turret Info")]
    public int totalTurrets = 6; // Total available turrets
    private int activeTurrets = 0; // How many are active

    [Header("UI")]
    public Text turretCountText; // <-- Legacy Text, not TextMeshProUGUI

    private void Start()
    {
        UpdateUI();
    }

    // Call this when a turret gets activated
    public void TurretActivated()
    {
        activeTurrets = Mathf.Clamp(activeTurrets + 1, 0, totalTurrets);
        UpdateUI();
    }

    // Call this when a turret gets deactivated
    public void TurretDeactivated()
    {
        activeTurrets = Mathf.Clamp(activeTurrets - 1, 0, totalTurrets);
        UpdateUI();
    }

    // Update the UI text
    private void UpdateUI()
    {
        if (turretCountText != null)
        {
            //turretCountText.text = "active turrets: " + activeTurrets + " / " + totalTurrets;
            turretCountText.text = activeTurrets + " active turrets";
        }
        else
        {
            Debug.LogWarning("TurretCountText UI element is not assigned!");
        }
    }
}
