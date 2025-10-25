using UnityEngine;

/// <summary>
/// Controls the enemy's movement, behavior, and interactions with the player.
/// The enemy moves toward the player, stops when close, takes damage, and dies with a death effect.
/// On death, it updates game statistics and triggers a car spawn.
/// </summary>
public class EnemyAI : MonoBehaviour
{
    [Header("Enemy Attributes")]
    public float maxEnemyToPlayerDistance;
    public float moveSpeed;
    public float health;
    public float rayLength;
    public LayerMask enemyMask;
    public GameObject deathEffect;

    [Header("Player References")]
    public GameObject player;

    private Rigidbody rigidBody;

    /// <summary>
    /// Initialises the Rigidbody component and locks rotation to prevent tipping.
    /// </summary>
    private void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        rigidBody.freezeRotation = true;
    }

    /// <summary>
    /// Rotates the enemy to face the player every frame.
    /// </summary>
    private void Update()
    {
        RotateToPlayer();
    }

    /// <summary>
    /// Moves the enemy toward the player at fixed intervals for consistent physics behavior.
    /// </summary>
    private void FixedUpdate()
    {
        MoveToPlayer();
    }

    /// <summary>
    /// Rotates the enemy to always face the player's position (on the horizontal plane only).
    /// </summary>
    private void RotateToPlayer()
    {
        Vector3 lookPosition = player.transform.position;

        lookPosition.y = transform.position.y;

        transform.LookAt(lookPosition);
    }

    /// <summary>
    /// Sets the reference to the player GameObject.
    /// </summary>
    /// <param name="player">The player GameObject to track.</param>
    public void SetPlayer(GameObject player)
    {
        this.player = player;
    }

    /// <summary>
    /// Reduces the enemy's health by one unit and triggers death if health reaches zero.
    /// </summary>
    public void TakeDamage()
    {
        --health;

        if (health <= 0)
        {
            OnDeath(true);
        }
    }

    /// <summary>
    /// Handles enemy death: spawns the death effect, destroys the enemy, updates game stats, and spawns a car.
    /// </summary>
    /// <param name="shouldUpdateGameStats">If true, updates game stats and spawns a new car.</param>
    public void OnDeath(bool shouldUpdateGameStats)
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);

        if (shouldUpdateGameStats)
        {
            GameObject gameStatsObject = GameObject.Find("GameStats");
            GameStatsPub gameStatsPub = gameStatsObject.GetComponent<GameStatsPub>();
            gameStatsPub.OnEnemyKilled();

            GameObject carSpawnerObject = GameObject.Find("CarSpawner");
            CarSpawner carSpawner = carSpawnerObject.GetComponent<CarSpawner>();
            carSpawner.SpawnCar();
        }
    }

    /// <summary>
    /// Checks whether the enemy is currently moving based on its Rigidbody velocity.
    /// </summary>
    /// <returns>True if the enemy is moving above a minimal threshold.</returns>
    public bool IsMoving()
    {
        return rigidBody.linearVelocity.magnitude > moveSpeed - Mathf.Epsilon;
    }

    /// <summary>
    /// Determines whether the enemy is close enough to the player to stop moving.
    /// </summary>
    /// <param name="toPlayer">Vector from the enemy to the player.</param>
    /// <returns>True if the player is within stopping distance.</returns>
    private bool IsEnemyCloseToPlayer(Vector3 toPlayer)
    {
        return toPlayer.magnitude < maxEnemyToPlayerDistance;
    }

    /// <summary>
    /// Checks if another enemy is directly in front using a raycast.
    /// Prevents overlapping movement.
    /// </summary>
    /// <returns>True if another enemy is detected ahead.</returns>
    private bool IsEnemyInFront()
    {
        Collider collider = GetComponent<Collider>();

        Vector3 origin = collider.bounds.center;
        Vector3 direction = transform.forward;

        Ray ray = new Ray(origin, direction);

        Debug.DrawRay(origin, direction * rayLength, Color.red);

        return Physics.Raycast(ray, out RaycastHit hit, rayLength, enemyMask);
    }

    /// <summary>
    /// Moves the enemy toward the player, stopping if too close or blocked by another enemy.
    /// </summary>
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
