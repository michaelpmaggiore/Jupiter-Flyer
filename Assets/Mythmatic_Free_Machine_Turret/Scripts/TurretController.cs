using System.ComponentModel;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Controls a turret that can rotate its base and weapon to track and shoot at enemies.
/// The turret has two parts: a rotating base and a weapon that can aim up/down.
/// Supports multiple projectile spawn points for simultaneous firing and explosion effects on impact.
/// </summary> 


    public class TurretController : MonoBehaviour
    {
        // Shooting Behavior
        public float bulletSpeed = 100f;
        public float bulletLifetime = 5f;
        public float attackTime = 40f; // Time active before deactivation
    public float shootingDelay = 1f; // Time between shots


        // Aiming & Rotation
        public float baseRotationSpeed = 180f;
        public float alignmentThreshold = 0.1f;
        public float weaponRotationSpeed = 180f;

        // GameObjects
        [Tooltip("Physical part of the turret that aims up/down")]
        public Transform weaponMount;
        [Tooltip("Target point for aiming calculations (usually at barrel tip)")]
        public Transform aimReference;
        [Tooltip("Projectile object to create when firing")]
        public GameObject projectilePrefab;

        // Audio Sources
        [Tooltip("AudioSource component to play the laser sound.")]
        public AudioSource shootingAudioSource;
        [Tooltip("AudioSource component to play the startup sound")]
        public AudioSource startupAudioSource;
        [Tooltip("AudioSource component to play the shutdown sound")]
        public AudioSource shutdownAudioSource;

        // Audio clips
        [Tooltip("Laser sound effect AudioClip.")]
        public AudioClip laserSound;
        [Tooltip("Startup sound effect AudioClip.")]
        public AudioClip startupSound;
        [Tooltip("Shutdown sound effect AudioClip.")]
        public AudioClip shutdownSound;




        // Private tracking variables
        private GameObject enemy = null;                    // Current target
        private bool isBaseRotating = false;         // Is the base currently rotating?
        private bool isWeaponRotating = false;       // Is the weapon currently rotating?
        private float targetWeaponAngle = 0f;        // Target angle for weapon rotation
        private float currentWeaponAngle = 0f;       // Current weapon angle
        private bool isActive = false;               // Whether the turret is online
        private float timeLeftShooting = 0f;
        private float shootCountdown = 0f;           // Countdown for next shot


        private void Start()
        {
            //Activate(); // temporary for testing

            // Ensure the AudioSources are set up correctly.
            if (shootingAudioSource != null && laserSound != null)
            {
                shootingAudioSource.clip = laserSound;
                shootingAudioSource.loop = false;
                shootingAudioSource.volume = 0.1f; // Adjust volume as needed
            }
            else
            {
                Debug.LogWarning("AudioSource or LaserSound AudioClip is missing!");
            }
            if (startupAudioSource != null && startupSound != null)
            {
                startupAudioSource.clip = startupSound;
                startupAudioSource.loop = false;
                startupAudioSource.volume = 2f; // Adjust volume as needed
            }
            else
            {
                Debug.LogWarning("AudioSource or StartupSound AudioClip is missing!");
            }
            if (shutdownAudioSource != null && shutdownSound != null)
            {
                shutdownAudioSource.clip = shutdownSound;
                shutdownAudioSource.loop = false;
                shutdownAudioSource.volume = 2f; // Adjust volume as needed
            }
            else
            {
                Debug.LogWarning("AudioSource or ShutdownSound AudioClip is missing!");
            }
        }

        // Called when turret is activated
        public void Activate()
        {
            isActive = true;
            timeLeftShooting = attackTime;
            startupAudioSource.Play();
        FindObjectOfType<ActiveTurretCount>()?.TurretActivated();

    }

    // Called when turret is deactivated
    private void Deactivate()
    {
        isActive = false;
        shutdownAudioSource.Play();
        FindObjectOfType<ActiveTurretCount>()?.TurretDeactivated();

    }

    // Returns the state of the turret
    public bool IsActive()
    {
        return isActive;
    }

        // Selects an enemy from all available enemies
        private void SelectEnemy()
        {
            AsteroidMarker[] markers = FindObjectsByType<AsteroidMarker>(FindObjectsSortMode.None);
            List<GameObject> enemies = new List<GameObject>();

            foreach (var marker in markers)
            {
                enemies.Add(marker.gameObject);
            }

            // Select the closest enemy to the turret
            if (enemies.Count > 0)
            {
                enemy = enemies.OrderBy(e => Vector3.Distance(transform.position, e.transform.position)).FirstOrDefault();
                //Debug.Log("Selected enemy at position X: " + enemy.transform.position.x + " Y: " + enemy.transform.position.y + " Z: " + enemy.transform.position.z);

            }
            else
            {
                Debug.LogWarning("No enemies found!");
                enemy = null;
            }
        }


        private void Update()
        {
            if (isActive)
            {
                //Debug.Log("Turret is active. time left: " + timeLeftShooting);
                // Update turret state
                if (timeLeftShooting <= 0)
                {
                    Deactivate();
                    return;
                }
                else
                {
                    timeLeftShooting -= Time.deltaTime;
                }

                // Update enemy and turret rotation
                SelectEnemy();
                RotateTowardsEnemy();

                // Shooting logic
                if (shootCountdown <= 0)
                {
                    Shoot();
                    shootCountdown = shootingDelay;
                }
                else
                {
                    shootCountdown -= Time.deltaTime;
                }
            }
        }

    private void RotateTowardsEnemy()
    {
        if (enemy == null)
        {
            Debug.LogWarning("No enemy selected for turret rotation.");
            return;
        }

        Vector3 toEnemy = enemy.transform.position - transform.position;

        // Flatten the enemy direction onto the plane of the turret's "up" axis
        Vector3 turretUp = transform.up;
        Vector3 flatToEnemy = Vector3.ProjectOnPlane(toEnemy, turretUp);

        if (flatToEnemy.sqrMagnitude < 0.001f)
            return; // Defensive check

        Vector3 flatDirection = flatToEnemy.normalized;

        Quaternion targetBaseRotation = Quaternion.LookRotation(flatDirection, turretUp);

        if (!Mathf.Approximately(Quaternion.Angle(transform.rotation, targetBaseRotation), 0f))
        {
            isBaseRotating = true;
            RotateBase(targetBaseRotation);
        }
        else if (isBaseRotating)
        {
            isBaseRotating = false;
            CalculateWeaponRotation();
        }

        if (!isBaseRotating)
        {
            CalculateWeaponRotation();
            if (isWeaponRotating)
            {
                RotateWeapon();
            }
        }
    }


    // Shoots a projectile from the turret
    private void Shoot()
        {
            GameObject projectile = Instantiate(projectilePrefab, aimReference.position, aimReference.rotation);
            Destroy(projectile, bulletLifetime);
            shootingAudioSource.Play();

            // Move manually instead of using Rigidbody
            BulletMovement bm = projectile.GetComponent<BulletMovement>();
            if (bm != null)
            {
                Vector3 shootDirection = aimReference.forward;
                bm.velocity = shootDirection * bulletSpeed;
            }
            else
            {
                Debug.LogError("Projectile does not have a BulletMovement script!");
            }
        }
        

        // Not my code
        private void RotateBase(Quaternion targetRotation)
        {
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                targetRotation,
                baseRotationSpeed * Time.deltaTime
            );
        }

        // Not my code
        private void CalculateWeaponRotation()
        {
            Vector3 toEnemy = enemy.transform.position - aimReference.position;
            Vector3 localToEnemy = transform.InverseTransformDirection(toEnemy);
            Vector3 localAimForward = transform.InverseTransformDirection(aimReference.forward);

            float targetAngle = -Mathf.Atan2(localToEnemy.y, localToEnemy.z) * Mathf.Rad2Deg;
            currentWeaponAngle = -Mathf.Atan2(localAimForward.y, localAimForward.z) * Mathf.Rad2Deg;

            float angleDifference = Mathf.DeltaAngle(currentWeaponAngle, targetAngle);

            if (Mathf.Abs(angleDifference) > alignmentThreshold)
            {
                isWeaponRotating = true;
                targetWeaponAngle = targetAngle;
            }
            else
            {
                isWeaponRotating = false;
            }
        }

        // Not my code
        private void RotateWeapon()
        {
            // Calculate the shortest rotation path
            float angleDifference = Mathf.DeltaAngle(currentWeaponAngle, targetWeaponAngle);

            // Calculate maximum rotation this frame based on rotation speed
            float maxRotationThisFrame = weaponRotationSpeed * Time.deltaTime;

            // Determine actual rotation amount, clamped by speed
            float rotationAmount = Mathf.Clamp(angleDifference, -maxRotationThisFrame, maxRotationThisFrame);

            // Apply rotation
            weaponMount.localRotation *= Quaternion.Euler(rotationAmount, 0, 0);
            currentWeaponAngle += rotationAmount;

            // Check if we've reached the target angle
            if (Mathf.Abs(angleDifference) <= alignmentThreshold)
            {
                isWeaponRotating = false;
            }
        }     
    }
