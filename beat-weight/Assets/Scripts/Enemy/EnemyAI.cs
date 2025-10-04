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

    public GameObject deathEffect;

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
        Vector3 lookPosition = player.transform.position;

        lookPosition.y = transform.position.y;

        transform.LookAt(lookPosition);
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
            OnDeath();
        }
    }

    private void OnDeath()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);

        // Update game stats
        GameObject gameStatsObject = GameObject.Find("GameStats");

        GameStatsPub gameStatsPub = gameStatsObject.GetComponent<GameStatsPub>();

        gameStatsPub.OnSuccessfulRep();
    }

    private bool IsEnemyCloseToPlayer(Vector3 toPlayer)
    {
        return toPlayer.magnitude < maxEnemyToPlayerDistance;
    }

    private bool IsEnemyInFront()
    {
        Collider collider = GetComponent<Collider>();

        Vector3 origin = collider.bounds.center;

        Vector3 direction = transform.forward;

        Ray ray = new Ray(origin, direction);

        Debug.DrawRay(origin, direction * rayLength, Color.red);

        return Physics.Raycast(ray, out RaycastHit hit, rayLength, enemyMask);
    }

    private void MoveToPlayer()
    {
        Vector3 toPlayer = player.transform.position - transform.position;

        toPlayer.Set(0, 0, toPlayer.z);

        Vector3 directionToPlayer = toPlayer.normalized;
        Vector3 velocity = moveSpeed * directionToPlayer;

        if (IsEnemyCloseToPlayer(toPlayer) || IsEnemyInFront())
        {
            velocity = Vector3.zero;
        }

        rigidBody.linearVelocity = velocity;
    }
}
