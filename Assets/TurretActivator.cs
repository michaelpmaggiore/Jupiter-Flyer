//using UnityEngine;
//using UnityEngine.UI; // For UI popup

//public class TurretActivator : MonoBehaviour
//{
//    public float activationRadius = 10f;
//    public TurretController turretController;
//    public Transform player;
//    public GameObject activationUIPrompt;
//    public KeyCode activationKey = KeyCode.E;
//    public Transform turret;

//    private bool showingPrompt = false;

//    private void Update()
//    {
//        if (player == null || turretController == null)
//            return;

//        float distance = Vector3.Distance(turret.position, player.position);

//        // Logic for deciding when to show the prompt
//        if (!turretController.IsActive() && distance <= activationRadius)
//        {
//            showingPrompt = true;
//        }
//        else
//        {
//            showingPrompt = false;
//        }

//        // Logic for showing the prompt and activating
//        if (showingPrompt)
//        {

//            activationUIPrompt?.SetActive(true);
//            if (Input.GetKeyDown(activationKey))
//            {
//                showingPrompt = false;
//                turretController.Activate();
//            }
//        }
//        else
//        {
//            activationUIPrompt?.SetActive(false);
//        }
//    }
//}

using UnityEngine;

namespace Mythmatic.TurretSystem
{
    public class TurretActivator : MonoBehaviour
    {
        public Transform turret;
        public Transform player;
        public TurretController turretController;
        public GameObject activationUIPrompt;
        public KeyCode activationKey = KeyCode.E;
        public float activationRadius = 5f;

        private static TurretActivator activePromptTurret = null; // <--- static!
        private bool showingPrompt = false;

        private void Update()
        {
            if (player == null || turretController == null)
                return;

            float distance = Vector3.Distance(turret.position, player.position);

            if (!turretController.IsActive() && distance <= activationRadius)
            {
                // Check if this turret should control the prompt
                if (activePromptTurret == null ||
                    Vector3.Distance(player.position, turret.position) < Vector3.Distance(player.position, activePromptTurret.turret.position))
                {
                    if (activePromptTurret != this)
                    {
                        // Disable old prompt
                        activePromptTurret?.HidePrompt();
                        activePromptTurret = this;
                        ShowPrompt();
                    }
                }
            }
            else
            {
                if (activePromptTurret == this)
                {
                    HidePrompt();
                    activePromptTurret = null;
                }
            }

            // Handle activation
            if (activePromptTurret == this && Input.GetKeyDown(activationKey))
            {
                turretController.Activate();
                HidePrompt();
                activePromptTurret = null;
            }
        }

        private void ShowPrompt()
        {
            showingPrompt = true;
            activationUIPrompt?.SetActive(true);
        }

        private void HidePrompt()
        {
            showingPrompt = false;
            activationUIPrompt?.SetActive(false);
        }
    }
}


