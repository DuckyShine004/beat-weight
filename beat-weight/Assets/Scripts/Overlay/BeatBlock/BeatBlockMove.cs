using UnityEngine;

public class BeatBlockMove : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public float moveSpeed = 700;
    public float deadZone = -100;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += moveSpeed * Time.deltaTime * Vector3.down;
        if (transform.position.y < deadZone)
        {
            KillBlock();
        }
    }

    public void KillBlock()
    {
        Destroy(gameObject);
    }
}
