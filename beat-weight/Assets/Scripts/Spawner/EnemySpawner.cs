using System;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawner Attributes")]
    public float spawnRate;

    public GameObject enemyModel;
    public GameObject player;

    public float enemyPadding;

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

    private void UpdateEnemyPositions()
    {
        // GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        //
        // // Sort the array by distance to player
        // Array.Sort(
        //     enemies,
        //     (a, b) =>
        //         Vector3
        //             .Distance(player.transform.position, a.transform.position)
        //             .CompareTo(Vector3.Distance(player.transform.position, b.transform.position))
        // );
        //
        // if (enemies.Length == 0) {
        //     return;
        // }
        //
        // BoxCollider collider = enemies[0].GetComponent<BoxCollider>();
        //
        // float enemyOffset = collider.size.z + enemyPadding;
        //
        // // For each of those enemies, displace manually
        // for (int i = 1; i < enemies.Length; ++i)
        // {
        //     if (enemies[i-1])
        // }
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnRate)
        {
            SpawnEnemy();

            timer = 0.0f;
        }

        UpdateEnemyPositions();
    }
}
