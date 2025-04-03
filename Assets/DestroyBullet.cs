using UnityEngine;

public class DestroyBullet : MonoBehaviour
{
    public float lifetime = 5f; // You can set this in the Inspector

    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}