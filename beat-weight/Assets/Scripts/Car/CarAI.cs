using UnityEngine;

public class CarAI : MonoBehaviour
{
    [Header("Car attributes")]
    private const int DROP_OFF_ZONE = 1;
    private const int END_POINT = 2;

    public float moveSpeed;

    private Rigidbody rigidBody;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector3 velocity = moveSpeed * transform.forward;

        rigidBody.linearVelocity = velocity;
    }
}
