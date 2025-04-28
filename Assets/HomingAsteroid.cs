using UnityEngine;

public class HomingAsteroid : MonoBehaviour
{
    [Tooltip("Target the asteroid should home towards.")]
    public Transform target;

    [Tooltip("Speed of the asteroid.")]
    public float speed = 10f;

    [Tooltip("Random spread angle for trajectory.")]
    public float randomSpreadAngle = 10f;

    private Vector3 moveDirection;

    private void Start()
    {
        if (target != null)
        {
            InitializeMovement();
        }
        else
        {
            Debug.LogWarning("HomingAsteroid: No target assigned.");
        }
    }

    private void Update()
    {
        if (target == null)
            return;

        transform.position += moveDirection * speed * Time.deltaTime;
    }

    private void InitializeMovement()
    {
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        moveDirection = ApplyRandomSpread(directionToTarget, randomSpreadAngle);
    }

    private Vector3 ApplyRandomSpread(Vector3 direction, float maxAngle)
    {
        return Quaternion.AngleAxis(Random.Range(-maxAngle, maxAngle), Random.insideUnitSphere) * direction;
    }
}
