using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
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
        Vector2 screenCentre = new Vector2(Screen.width * 0.5f, Screen.height * 0.5f);

        Ray ray = Camera.main.ScreenPointToRay(screenCentre);

        Vector3 bulletDirection = ray.direction.normalized;

        GameObject bullet = Instantiate(bulletModel, muzzle.position, Quaternion.LookRotation(bulletDirection));

        bullet.GetComponent<Rigidbody>().AddForce(bulletDirection * bulletSpeed, ForceMode.Impulse);
    }
}
