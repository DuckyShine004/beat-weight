using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet attributes")]
    public float lifeTime = 3.0f;

    private bool isCollision;

    public void Start()
    {
        isCollision = false;
    }

    private void OnEnable()
    {
        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(gameObject);

            EnemyAI enemyAI = other.GetComponent<EnemyAI>();

            enemyAI.TakeDamage();

            isCollision = true;
        }
    }

    private void OnDestroy()
    {
        if (!isCollision)
        {
            GameObject gameStatsObject = GameObject.Find("GameStats");

            GameStatsPub gameStatsPub = gameStatsObject.GetComponent<GameStatsPub>();

            gameStatsPub.OnFailedRep();
        }
    }
}
