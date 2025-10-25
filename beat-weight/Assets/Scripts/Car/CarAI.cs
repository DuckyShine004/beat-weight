using UnityEngine;

/// <summary>
/// Simple car AI controller that continuously moves the car forward at a constant speed.
/// The car uses a Rigidbody for physics-based motion.
/// </summary>
public class CarAI : MonoBehaviour
{
    [Header("Car attributes")]
    private const int DROP_OFF_ZONE = 1;
    private const int END_POINT = 2;

    public float moveSpeed;

    private Rigidbody rigidBody;

    /// <summary>
    /// Initialises the Rigidbody and locks its rotation to prevent tipping.
    /// </summary>
    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.freezeRotation = true;
    }

    /// <summary>
    /// Called at a fixed timestep for consistent physics updates.
    /// Handles movement.
    /// </summary>
    private void FixedUpdate()
    {
        Move();
    }

    /// <summary>
    /// Moves the car forward at the specified speed using physics velocity.
    /// </summary>
    private void Move()
    {
        Vector3 velocity = moveSpeed * transform.forward;

        rigidBody.linearVelocity = velocity;
    }
}
