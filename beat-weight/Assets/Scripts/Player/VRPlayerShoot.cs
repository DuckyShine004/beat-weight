using UnityEngine;
using UnityEngine.InputSystem;

public class VRTriggerShoot : MonoBehaviour
{
    [Header("References")]
    public Transform muzzle;
    public GameObject bulletPrefab;
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor rayInteractor;

    [Header("Ballistics")]
    public float bulletSpeed = 20f;
    public float fallbackDistance = 50f;

    public float rotationForce = 20f;

    [Header("Trigger Settings")]
    [Range(0f, 1f)] public float fireThreshold = 0.75f;

    private InputAction _triggerAction;
    private bool _wasPressed = false;

    private void OnEnable()
    {
        _triggerAction = new InputAction("Trigger", binding: "<XRController>{RightHand}/trigger");
        _triggerAction.AddBinding("<OculusTouchController>{RightHand}/trigger");
        _triggerAction.Enable();
    }

    private void OnDisable()
    {
        _triggerAction.Disable();
        _triggerAction.Dispose();
    }

    private void Update()
    {
        if (!_triggerAction.enabled || !muzzle || !bulletPrefab || !rayInteractor) return;

        float value = _triggerAction.ReadValue<float>();
        bool isPressed = value >= fireThreshold;

        if (isPressed && !_wasPressed)
        {
            ShootAlongLine();
        }

        _wasPressed = isPressed;
    }

    private void ShootAlongLine()
    {
        Vector3 aimPoint;

        // Use the same ray as the XR Interactor Line Visual
        if (rayInteractor.TryGetCurrent3DRaycastHit(out RaycastHit hit))
            aimPoint = hit.point;
        else
            aimPoint = muzzle.position + rayInteractor.transform.forward * fallbackDistance;

        Vector3 dir = (aimPoint - muzzle.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, muzzle.position, Quaternion.LookRotation(dir));
        if (bullet.TryGetComponent<Rigidbody>(out var rb))
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rb.AddForce(dir * bulletSpeed, ForceMode.Impulse);

            // 🎯 Add spin here
            rb.AddTorque(Random.insideUnitSphere * rotationForce, ForceMode.Impulse);
        }
        else
        {
            Debug.LogWarning("Bullet prefab needs a Rigidbody.");
        }
    }
}
