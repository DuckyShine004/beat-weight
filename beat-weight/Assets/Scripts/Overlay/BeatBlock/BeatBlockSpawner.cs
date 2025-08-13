using System.IO.Pipes;
using UnityEngine;

public class BeatBlockSpawner : MonoBehaviour
{
    public GameObject beatBlock;
    public float spawnRate = 2;
    private float timer = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SpawnBeatBlock();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < spawnRate)
        {
            timer += Time.deltaTime;
        }
        else
        {
            SpawnBeatBlock();
            timer = 0;
        }

    }

    private void SpawnBeatBlock()
    {
        Instantiate(beatBlock, transform.position, transform.rotation, transform);
    }
}
