using UnityEngine;

public class DropOff : MonoBehaviour
{
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
