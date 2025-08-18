using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Attributes")]
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

        enemy.GetComponent<EnemyAI>().player = player;
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
