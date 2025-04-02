using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Tooltip("The target (player or ball) that the camera will follow.")]
    public Transform player;
    
    [Tooltip("Mouse sensitivity for camera rotation.")]
    public float sensitivity = 3.0f;
    
    [Tooltip("Distance from the player.")]
    public float distance = 10f;

    private float yaw = 0f;   // Horizontal rotation angle.
    private float pitch = 0f; // Vertical rotation angle.

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("Player transform is not assigned in the CameraController!");
            return;
        }

        // Initialize yaw and pitch from the current camera rotation.
        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;

        // Lock and hide the cursor for camera control.
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        if (player == null)
            return;

        // Get mouse input.
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        // Update yaw and pitch directly.
        yaw += mouseX;
        pitch -= mouseY;  // Inverting mouseY so that moving the mouse upward raises the view

        // Compute the desired rotation based on yaw and pitch.
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        // Calculate the offset behind the player at the given distance.
        Vector3 offset = rotation * new Vector3(0, 0, -distance);

        // Set the camera's position relative to the player's position.
        transform.position = player.position + offset;

        // Set the camera's rotation.
        transform.rotation = rotation;
    }
}
