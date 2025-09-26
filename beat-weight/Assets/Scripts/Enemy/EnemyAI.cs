using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Enemy attributes")]
    public float maxEnemyToPlayerDistance;

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

    private bool IsEnemyCloseToPlayer(Vector3 toPlayer)
    {
        return toPlayer.magnitude < maxEnemyToPlayerDistance;
    }

    private void MoveToPlayer()
    {
        Vector3 toPlayer = player.transform.position - transform.position;
        Vector3 directionToPlayer = toPlayer.normalized;

        Vector3 velocity = moveSpeed * directionToPlayer;

        if (IsEnemyCloseToPlayer(toPlayer))
        {
            velocity = Vector3.zero;
        }

        rigidBody.linearVelocity = velocity;
        Debug.DrawRay(transform.position, directionToPlayer, Color.red);
    }
}
