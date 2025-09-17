using System.IO.Pipes;
using UnityEngine;

public class BeatBlockSpawner : MonoBehaviour
{
    public GameObject upBeatBlock;
    public GameObject downBeatBlock;
    public float spawnRate;
    public float delay;
    private float timer;

    private bool toggle = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = delay;
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
        if (toggle)
        {
            Instantiate(upBeatBlock, transform.position, transform.rotation, transform);
        }
        else
        {
            Instantiate(downBeatBlock, transform.position, transform.rotation, transform);
        }
        toggle = !toggle;
    }
}
