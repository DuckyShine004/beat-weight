using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Attributes")]
    public float spawnRate;

    public GameObject enemyModel;
    public GameObject player;

    private float timer;

    void Start()
    {
        timer = 0.0f;
    }

    private void SpawnEnemy()
    {
        GameObject enemy = Instantiate(enemyModel, transform.position, Quaternion.identity);

        EnemyAI enemyAI = enemy.GetComponent<EnemyAI>();

        enemyAI.SetPlayer(player);
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnRate)
        {
            SpawnEnemy();

            timer = 0.0f;
        }
    }
}
