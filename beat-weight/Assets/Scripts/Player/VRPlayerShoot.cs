using UnityEngine;
using UnityEngine.InputSystem;

public class VRTriggerShoot : MonoBehaviour
{
    [Header("Refs")]
    public Transform fireOrigin;       // optional; defaults to this.transform
    public GameObject bulletPrefab;    // must have a Rigidbody

    [Header("Ballistics")]
    public float bulletSpeed = 20f;
    public float spinTorque = 20f;

    public void ShootStraight()
    {
        var origin = fireOrigin ? fireOrigin : transform;
        if (!bulletPrefab) return;
        
        origin.position -= Vector3.up * 0.1f;

        var bullet = Instantiate(bulletPrefab, origin.position, origin.rotation);
        if (bullet.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.AddForce(origin.forward * bulletSpeed, ForceMode.Impulse);
            rb.AddTorque(Random.insideUnitSphere * spinTorque, ForceMode.Impulse);
        }
    }

    // Optional: shoot in an explicit direction
    public void ShootDir(Vector3 dir)
    {
        var origin = fireOrigin ? fireOrigin : transform;
        if (!bulletPrefab) return;
        origin.position -= Vector3.up * 0.1f;

        dir = dir.sqrMagnitude > 1e-6f ? dir.normalized : origin.forward;
        var bullet = Instantiate(bulletPrefab, origin.position, Quaternion.LookRotation(dir));
        if (bullet.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.AddForce(dir * bulletSpeed, ForceMode.Impulse);
            rb.AddTorque(Random.insideUnitSphere * spinTorque, ForceMode.Impulse);
        }
    }

    // worldDir = direction in WORLD space (doesn't need to be normalized)
    public void ShootWorldDir(Vector3 worldDir)
    {
        var origin = fireOrigin ? fireOrigin : transform;
        if (!bulletPrefab) return;
        origin.position -= Vector3.up * 0.4f;
        Vector3 dir = worldDir.sqrMagnitude < 1e-6f ? origin.forward : worldDir.normalized;

        var bullet = Instantiate(bulletPrefab, origin.position, Quaternion.LookRotation(dir));
        if (bullet.TryGetComponent<Rigidbody>(out var rb))
            rb.AddForce(dir * bulletSpeed, ForceMode.Impulse);
    }

}