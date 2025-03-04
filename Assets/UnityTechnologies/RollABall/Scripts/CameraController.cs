using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;  // Assign your ball (or player) in the Inspector
    public float sensitivity = 3.0f; // Mouse sensitivity (adjust as needed)
    public float minPitch = -30f;    // Lower limit for vertical rotation
    public float maxPitch = 60f;     // Upper limit for vertical rotation

    private float yaw = 0f;   // Horizontal rotation angle
    private float pitch = 0f; // Vertical rotation angle
    private Vector3 initialOffset; // The starting offset between the camera and the player

    void Start()
    {
        // Ensure the player transform is assigned
        if (player == null)
        {
            Debug.LogError("Player transform is not assigned in the CameraController!");
            return;
        }

        // Calculate the initial offset from the player to the camera
        initialOffset = transform.position - player.position;
        
        // Optionally, initialize yaw and pitch based on current camera rotation
        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;

        // Lock and hide the cursor so the mouse controls the camera
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void LateUpdate()
    {
        // Ensure we have a player assigned
        if (player == null)
            return;

        // Get mouse input from the horizontal and vertical axes
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;

        // Update rotation angles (yaw rotates horizontally, pitch rotates vertically)
        yaw += mouseX;
        pitch = Mathf.Clamp(pitch - mouseY, minPitch, maxPitch);

        // Create a rotation from the yaw and pitch
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);

        // Rotate the initial offset by this rotation and add it to the player's position.
        Vector3 desiredPosition = player.position + rotation * initialOffset;

        // Update the camera position so it follows the player with the rotated offset.
        transform.position = desiredPosition;

        // Make the camera look at the player.
        transform.LookAt(player.position);

        // Debug logs (optional, remove in production)
        // Debug.Log("Yaw: " + yaw + " | Pitch: " + pitch + " | Desired Pos: " + desiredPosition);
    }
}
