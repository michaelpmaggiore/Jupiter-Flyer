using System.Collections.Generic;
using UnityEngine;

public class SpeedTrail : MonoBehaviour
{
    [Tooltip("Transform of the object to track. If left empty, targetRigidbody's transform is used.")]
    public Transform target;
    [Tooltip("Rigidbody of the object to track. Its transform will be used if target is not set.")]
    public Rigidbody targetRigidbody;

    [Tooltip("Tail length in meters (the line will always extend this distance behind the bullet).")]
    public float tailLength;

    [Tooltip("Color at the bullet (start of the trail).")]
    public Color startColor = Color.cyan;
    [Tooltip("Color at the tail end (where the trail dissipates).")]
    public Color endColor = new Color(0f, 1f, 1f, 0f);

    [Tooltip("Width at the bullet (start of the trail).")]
    public float startWidth = 0.2f;
    [Tooltip("Width at the tail end.")]
    public float endWidth = 0f;

    private LineRenderer lineRenderer;

    // List to store the trail positions (ordered from tail to bullet)
    private List<Vector3> trailPoints = new List<Vector3>();

    private void Awake()
    {
        // Ensure a LineRenderer is attached; add one if missing.
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        // Use a simple material that supports transparency.
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.useWorldSpace = true;
        lineRenderer.numCapVertices = 2; // Optional: makes rounded end caps.

        // Set up the color gradient.
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] {
                new GradientColorKey(startColor, 0f),
                new GradientColorKey(endColor, 1f)
            },
            new GradientAlphaKey[] {
                new GradientAlphaKey(startColor.a, 0f),
                new GradientAlphaKey(endColor.a, 1f)
            }
        );
        lineRenderer.colorGradient = gradient;

        // Set up the width curve.
        AnimationCurve widthCurve = new AnimationCurve();
        widthCurve.AddKey(0f, startWidth);
        widthCurve.AddKey(1f, endWidth);
        lineRenderer.widthCurve = widthCurve;
    }

    private void Start()
    {
        // If target isn't set, try using the Rigidbody's transform.
        if (target == null && targetRigidbody != null)
        {
            target = targetRigidbody.transform;
        }

        // Initialize the trail with the current position.
        if (target != null)
        {
            Vector3 pos = target.position;
            trailPoints.Clear();
            trailPoints.Add(pos);
        }
    }

    private void Update()
    {
        if (target == null)
            return;

        Vector3 currentPosition = target.position;

        // Append the bullet's current position if it has moved.
        if (trailPoints.Count == 0 || Vector3.Distance(trailPoints[trailPoints.Count - 1], currentPosition) > 0.001f)
        {
            trailPoints.Add(currentPosition);
        }

        // Process the trail to keep only the last 'tailLength' meters.
        TrimTrail();

        // Update the LineRenderer using the processed trail.
        lineRenderer.positionCount = trailPoints.Count;
        for (int i = 0; i < trailPoints.Count; i++)
        {
            lineRenderer.SetPosition(i, trailPoints[i]);
        }
    }

    /// <summary>
    /// Trims the trail so that the total length is never more than tailLength.
    /// It walks backward from the bullet's current position, accumulating distance,
    /// and interpolates the tail if needed.
    /// </summary>
    private void TrimTrail()
    {
        List<Vector3> newTrail = new List<Vector3>();

        // Start from the bullet (last point in trailPoints).
        float accumulatedDistance = 0f;
        newTrail.Add(trailPoints[trailPoints.Count - 1]); // current bullet position

        // Traverse backward through the stored positions.
        for (int i = trailPoints.Count - 2; i >= 0; i--)
        {
            float segmentDistance = Vector3.Distance(trailPoints[i + 1], trailPoints[i]);

            if (accumulatedDistance + segmentDistance >= tailLength)
            {
                // Compute the precise tail position by interpolating along this segment.
                float remaining = tailLength - accumulatedDistance;
                float t = remaining / segmentDistance;
                Vector3 interpolatedPoint = Vector3.Lerp(trailPoints[i + 1], trailPoints[i], t);
                newTrail.Add(interpolatedPoint);
                break;
            }
            else
            {
                newTrail.Add(trailPoints[i]);
                accumulatedDistance += segmentDistance;
            }
        }

        // newTrail was built from the bullet backward, so reverse it to have tail-to-bullet order.
        newTrail.Reverse();

        // Replace the old trail with the trimmed one.
        trailPoints = newTrail;
    }
}
