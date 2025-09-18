using UnityEngine;

public class AmmoVisibilityGate : MonoBehaviour
{
    public DataManager dataManager;             // assign your global store asset

    public GameObject[] ammoDependentObjects;   // objects to toggle based on ammo

    public GameObject noAmmoFallback;         // (optional) show this if out of ammo

    void Update()
    {
        if (!dataManager || ammoDependentObjects == null) return;

        float currentAmmo = dataManager.ammo;

        foreach (var obj in ammoDependentObjects)
        {
            if (obj) obj.SetActive(currentAmmo > 0);
            currentAmmo -= 1; // only show as many objects as you have ammo
        }
        if (noAmmoFallback) noAmmoFallback.SetActive(dataManager.ammo <= 0);
    }


}
