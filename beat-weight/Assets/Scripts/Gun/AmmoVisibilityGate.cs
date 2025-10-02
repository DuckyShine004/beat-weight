using UnityEngine;

public class AmmoVisibilityGate : MonoBehaviour
{
    public DataManager dataManager;   // assign your global store asset
    public HandManager handManager;   // assign your HandManager

    [Header("Ammo dependent objects")]
    public GameObject[] ammoDependentObjects;   // objects to toggle based on ammo count

    [Header("Fallbacks (Left Hand)")]
    public GameObject noAmmoFallbackLeft;   // show if ammo == 0
    public GameObject oneAmmoFallbackLeft;  // show if ammo == 1

    [Header("Fallbacks (Right Hand)")]
    public GameObject noAmmoFallbackRight;   // show if ammo == 0
    public GameObject oneAmmoFallbackRight;  // show if ammo == 1

    void Update()
    {
        if (!dataManager || ammoDependentObjects == null || handManager == null) return;

        float currentAmmo = dataManager.ammo;

        // Toggle ammo objects
        foreach (var obj in ammoDependentObjects)
        {
            if (obj) obj.SetActive(currentAmmo > 1);
            currentAmmo -= 1;
        }

        // Hand-based fallbacks
        bool isLeft = handManager.activeHand == HandManager.Hand.Left;

        if (!isLeft)
        {
            if (noAmmoFallbackLeft) noAmmoFallbackLeft.SetActive(dataManager.ammo == 0);
            if (oneAmmoFallbackLeft) oneAmmoFallbackLeft.SetActive(dataManager.ammo >= 1);

            // Make sure right-hand fallbacks are hidden
            if (noAmmoFallbackRight) noAmmoFallbackRight.SetActive(false);
            if (oneAmmoFallbackRight) oneAmmoFallbackRight.SetActive(false);
        }
        else // Right hand
        {
            if (noAmmoFallbackRight) noAmmoFallbackRight.SetActive(dataManager.ammo == 0);
            if (oneAmmoFallbackRight) oneAmmoFallbackRight.SetActive(dataManager.ammo >= 1);

            // Make sure left-hand fallbacks are hidden
            if (noAmmoFallbackLeft) noAmmoFallbackLeft.SetActive(false);
            if (oneAmmoFallbackLeft) oneAmmoFallbackLeft.SetActive(false);
        }
    }
}
