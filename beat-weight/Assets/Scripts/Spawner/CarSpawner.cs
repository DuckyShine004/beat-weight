using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [Header("Spawner Attributes")]
    public GameObject carModel;
    public float spawnRate;
    private float timer;

    [Header("Restaurant Attributes")]
    public GameObject restaurantEntrance;

    private void Start() {
        SpawnCar();
    }

    private bool CanSpawnCar()
    {
        EnemyAI[] enemies = FindObjectsByType<EnemyAI>(FindObjectsSortMode.None);

        Vector3 restaurantEntrancePosition = restaurantEntrance.transform.position;

        foreach (EnemyAI enemy in enemies)
        {
            Vector3 enemyPosition = enemy.transform.position;

            if (!enemy.IsMoving() && enemyPosition.z < restaurantEntrancePosition.z)
            {
                return false;
            }
        }

        return true;
    }

    public void SpawnCar()
    {
        if (CanSpawnCar())
        {
            Instantiate(carModel, transform.position, transform.rotation);
        }
    }
}
