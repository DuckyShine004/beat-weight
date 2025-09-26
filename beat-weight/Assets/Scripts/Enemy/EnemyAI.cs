using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Enemy attributes")]
    public float maxEnemyToPlayerDistance;

    public float moveSpeed;
    public float health;

    public float rayLength;

    public LayerMask enemyMask;

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

        Quaternion rotation = player.transform.rotation;

        transform.rotation.Set(0, rotation.y, 0, rotation.w);
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

    private bool IsEnemyInFront(Vector3 directionToPlayer)
    {
        Ray ray = new Ray(transform.position, directionToPlayer);

        Debug.DrawRay(transform.position, directionToPlayer * rayLength, Color.red);

        return Physics.Raycast(ray, rayLength, enemyMask);
    }

    private void MoveToPlayer()
    {
        Vector3 toPlayer = player.transform.position - transform.position;

        toPlayer.Set(0, 0, toPlayer.z);

        Vector3 directionToPlayer = toPlayer.normalized;
        Vector3 velocity = moveSpeed * directionToPlayer;

        if (IsEnemyCloseToPlayer(toPlayer) || IsEnemyInFront(directionToPlayer))
        {
            velocity = Vector3.zero;
        }

        rigidBody.linearVelocity = velocity;

        // Debug.DrawRay(transform.position, directionToPlayer, Color.red);
    }
}
