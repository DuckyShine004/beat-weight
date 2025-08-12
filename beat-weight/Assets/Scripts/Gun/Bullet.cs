using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime = 3.0f;

    private void Update()
    {
        BulletTimeToLive();
    }

    private void BulletTimeToLive()
    {
        lifeTime -= Time.deltaTime;

        if (lifeTime < 0)
        {
            Destroy(gameObject);
        }
    }

    // Collision detection here
    private void OnTriggerEnter(Collider other)
    {

    }
}
