using UnityEngine;

/// <summary>
/// Handles spawning of cars at a defined rate and position,
/// ensuring cars only spawn when no stationary enemies block the restaurant entrance.
/// </summary>
public class CarSpawner : MonoBehaviour
{
    [Header("Car Spawner Attributes")]
    public GameObject carModel;
    public float spawnRate;
    private float timer;

    [Header("References")]
    public GameObject restaurantEntrance;

    /// <summary>
    /// Called when the script starts.
    /// Immediately spawns the first car.
    /// </summary>
    private void Start()
    {
        SpawnCar();
    }

    /// <summary>
    /// Checks if it is safe to spawn a new car.
    /// Returns false if any enemy is stationary and positioned before the restaurant entrance.
    /// </summary>
    /// <returns>True if the spawn area is clear; otherwise, false.</returns>
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

    /// <summary>
    /// Attempts to spawn a new car prefab at the spawner's location and rotation.
    /// Only spawns if the area is clear, as determined by <see cref="CanSpawnCar"/>.
    /// </summary>
    public void SpawnCar()
    {
        if (CanSpawnCar())
        {
            Instantiate(carModel, transform.position, transform.rotation);
        }
    }
}
