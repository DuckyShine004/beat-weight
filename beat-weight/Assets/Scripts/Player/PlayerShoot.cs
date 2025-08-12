using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public Transform orientation;

    [Header("Model")]
    public GameObject bulletModel;


    void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    private void Shoot()
    {
        GameObject bullet = Instantiate(bulletModel, transform.position, Quaternion.identity);
    }
}
