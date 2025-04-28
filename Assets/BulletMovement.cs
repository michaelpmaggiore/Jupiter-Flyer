using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    public Vector3 velocity;

    private void Update()
    {
        transform.position += velocity * Time.unscaledDeltaTime;
    }
}
