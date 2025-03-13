using UnityEngine;
using System.Collections.Generic;

public class DynamicSpeedTrail : MonoBehaviour
{
    public float maxTrailLength = 5f; // Maximum length of the trail
    public float speedMultiplier = 1f; // Adjusts how much speed affects the trail length
    public int maxPoints = 50; // Maximum number of points in the line

    private LineRenderer lineRenderer;
    private Rigidbody rb;
    private List<Vector3> trailPositions = new List<Vector3>();

    private void Start()
    {
        // Get Rigidbody (Ensure object has one)
        rb = GetComponent<Rigidbody>();

        // Add and configure LineRenderer
        lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default")); // Simple material
        lineRenderer.positionCount = 0;
    }

    private void Update()
    {
        if (rb == null) return;

        // Get the object's speed
        float speed = rb.linearVelocity.magnitude;

        // Calculate the max length of the trail based on speed
        float currentTrailLength = Mathf.Clamp(speed * speedMultiplier, 1f, maxTrailLength);

        // Add the current position to the list
        trailPositions.Add(transform.position);

        // Ensure trail does not exceed the maximum allowed length
        while (GetTrailLength() > currentTrailLength && trailPositions.Count > 1)
        {
            trailPositions.RemoveAt(0); // Remove the oldest position to maintain length
        }

        // Update LineRenderer points
        lineRenderer.positionCount = trailPositions.Count;
        lineRenderer.SetPositions(trailPositions.ToArray());
    }

    // Calculate the total length of the trail
    private float GetTrailLength()
    {
        float length = 0f;
        for (int i = 1; i < trailPositions.Count; i++)
        {
            length += Vector3.Distance(trailPositions[i - 1], trailPositions[i]);
        }
        return length;
    }
}
