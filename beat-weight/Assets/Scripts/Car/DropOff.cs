using UnityEngine;

/// <summary>
/// Handles the car drop-off trigger area.
/// When a car enters this trigger zone, it notifies the <see cref="EnemySpawner"/> to spawn a new enemy.
/// </summary>
public class DropOff : MonoBehaviour
{
    /// <summary>
    /// Called automatically by Unity when another collider enters this trigger.
    /// If the entering object has the tag "Car", it will trigger the enemy spawner to create a new enemy.
    /// </summary>
    /// <param name="other">The collider of the object that entered the trigger zone.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Car"))
        {
            GameObject enemySpawnerObject = GameObject.Find("EnemySpawner");

            EnemySpawner enemySpawner = enemySpawnerObject.GetComponent<EnemySpawner>();

            enemySpawner.SpawnEnemy();
        }
    }
}
