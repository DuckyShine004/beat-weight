using UnityEngine;

/// <summary>
/// Responsible for spawning enemy units at a fixed location
/// and assigning them a reference to the player for targeting and movement.
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Spawner Attributes")]
    public GameObject enemyModel;
    public GameObject player;

    /// <summary>
    /// Spawns a new enemy at the spawner's position and assigns its player reference.
    /// </summary>
    public void SpawnEnemy()
    {
        GameObject enemy = Instantiate(enemyModel, transform.position, Quaternion.identity);

        EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();

        enemyAI.SetPlayer(player);
    }
}
