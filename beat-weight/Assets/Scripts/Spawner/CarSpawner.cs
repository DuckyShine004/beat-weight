using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [Header("Spawner Attributes")]
    public GameObject carModel;

    public float spawnRate;

    private float timer;

    private void Start()
    {
        timer = 0.0f;
    }

    public void SpawnCar()
    {
        Instantiate(carModel, transform.position, transform.rotation);
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnRate)
        {
            SpawnCar();

            timer = 0.0f;
        }
    }
}
