using UnityEngine;

/// <summary>
/// Represents a bullet fired by the player.
/// Handles collisions with enemies, applies damage, and notifies the game stats system of hits or misses.
/// </summary>
public class Bullet : MonoBehaviour
{
    [Header("Bullet attributes")]
    public float lifeTime = 3.0f;

    private bool isCollision;

    /// <summary>
    /// Initialises the bullet's collision flag.
    /// </summary>
    private void Start()
    {
        isCollision = false;
    }

    /// <summary>
    /// Called when the bullet is enabled.
    /// Automatically destroys the bullet after its lifetime expires.
    /// </summary>
    private void OnEnable()
    {
        Destroy(gameObject, lifeTime);
    }

    /// <summary>
    /// Called when the bullet collides with another collider marked as a trigger.
    /// If the collider belongs to an enemy, apply damage and destroy the bullet.
    /// </summary>
    /// <param name="other">The collider that the bullet has entered.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(gameObject);

            EnemyAI enemyAI = other.GetComponent<EnemyAI>();
            enemyAI.TakeDamage();

            isCollision = true;
        }
    }

    /// <summary>
    /// Called automatically when the bullet is destroyed.
    /// If the bullet was destroyed without colliding (missed enemy),
    /// updates game stats to record a failed rep.
    /// </summary>
    private void OnDestroy()
    {
        if (!isCollision)
        {
            GameObject gameStatsObject = GameObject.Find("GameStats");
            GameStatsPub gameStatsPub = gameStatsObject.GetComponent<GameStatsPub>();

            gameStatsPub.OnFailedRep();
        }
    }
}
