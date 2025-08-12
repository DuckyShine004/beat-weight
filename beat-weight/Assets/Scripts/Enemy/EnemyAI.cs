using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed;

    public GameObject player;

    private Rigidbody rigidBody;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.freezeRotation = true;
    }

    private void FixedUpdate()
    {
        MoveToPlayer();
    }

    private void MoveToPlayer()
    {
        print(player.transform.position);
        Vector3 direction = (player.transform.position - transform.position).normalized;
        Vector3 velocity = moveSpeed * direction;

        rigidBody.linearVelocity = velocity;
        Debug.DrawRay(transform.position, direction, Color.red);
    }
}
