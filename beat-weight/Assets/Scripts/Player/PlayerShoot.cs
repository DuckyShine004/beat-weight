using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("Camera")]
    public Camera playerCamera;

    public Transform muzzle;

    [Header("Model")]
    public GameObject bulletModel;

    public float bulletSpeed;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }
    }

    // Currently just shoot to centre of screen
    private void Shoot()
    {
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));

        Vector3 bulletDirection = ray.direction.normalized;

        Quaternion bulletRotation = Quaternion.LookRotation(bulletDirection);

        GameObject bullet = Instantiate(bulletModel, muzzle.position, bulletRotation);

        Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();

        bulletRigidbody.linearVelocity = bulletDirection * bulletSpeed;
    }
}
