using UnityEngine;

/// <summary>
/// Handles moving the camera to the player's position.
/// </summary>
public class MoveCamera : MonoBehaviour
{
    [Header("Camera Position")]
    public Transform cameraPosition;

    /// <summary>
    /// On each frame, ensure that the player's camera is updated.
    /// </summary>
    private void Update()
    {
        transform.position = cameraPosition.position;
    }
}
