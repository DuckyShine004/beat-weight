using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Enemy attributes")]
    public float moveSpeed;
    public float health;

    public GameObject player;

    private Rigidbody rigidBody;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.freezeRotation = true;
    }

    private void Update()
    {
        RotateToPlayer();
    }

    private void FixedUpdate()
    {
        MoveToPlayer();
    }

    private void RotateToPlayer()
    {
        transform.LookAt(player.transform);
    }

    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }

    public void TakeDamage()
    {
        --health;

        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void MoveToPlayer()
    {
        print(player.transform.position);
        Vector3 direction = (player.transform.position - transform.position).normalized;
        direction.Set(direction.x, 0, direction.z);
        direction.Normalize();
        Vector3 velocity = moveSpeed * direction;

        rigidBody.linearVelocity = velocity;
        Debug.DrawRay(transform.position, direction, Color.red);
    }
}
