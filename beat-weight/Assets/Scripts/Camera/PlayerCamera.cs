using UnityEngine;

/// <summary>
/// Handles player camera rotation based on mouse input.
/// Locks the cursor and applies pitch/yaw rotation to the camera and orientation transform.
/// </summary>
public class PlayerCamera : MonoBehaviour
{
    [Header("Mouse Sensitivity")]
    public float sensitivityX;
    public float sensitivityY;

    [Header("References")]
    public Transform orientation;

    // Rotation around the X (pitch) and Y (Yaw) axes
    private float rotationX;
    private float rotationY;

    /// <summary>
    /// Locks the cursor to the game window and hides it.
    /// </summary>
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// Updates camera rotation each frame based on raw mouse input. The orientation variable
    /// is the player's current orientation.
    /// </summary>
    private void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensitivityX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensitivityY;

        rotationY += mouseX;
        rotationX -= mouseY;

        rotationX = Mathf.Clamp(rotationX, -90.0f, 90.0f);

        transform.rotation = Quaternion.Euler(rotationX, rotationY, 0);
        orientation.rotation = Quaternion.Euler(0, rotationY, 0);
    }
}
