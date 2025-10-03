using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Attributes")]
    public GameObject enemyModel;
    public GameObject player;

    public void SpawnEnemy()
    {
        GameObject enemy = Instantiate(enemyModel, transform.position, Quaternion.identity);

        EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();

        enemyAI.SetPlayer(player);
    }
}
