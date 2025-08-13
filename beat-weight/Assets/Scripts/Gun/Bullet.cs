using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 3.0f;

    private void OnEnable()
    {
        Destroy(gameObject, lifeTime);
    }

    // Collision detection here
    private void OnTriggerEnter(Collider other)
    {

    }
}
