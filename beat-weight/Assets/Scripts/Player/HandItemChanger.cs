using UnityEngine;

public class HandItemChanger : MonoBehaviour
{
    [Header("UI")]
    public RectTransform barArea;     // vertical track (parent)
    public RectTransform watchedDot;  // controller dot (child of barArea)
    public RectTransform beatDot;     // beat marker dot (child of barArea)

    [Header("Game/Data")]
    public DataManager dataManager;   // should have either public int ammo; or void AddAmmo(int)

    [Header("Equip targets")]
    public Transform handAnchor;      // (optional, unused here)
    public GameObject topVariant;     // active in top band
    public GameObject bottomVariant;  // active in bottom band

    [Header("Thresholds")]
    [Range(0f, 1f)] public float topThreshold = 0.80f;        // top 20%
    [Range(0f, 1f)] public float bottomThreshold = 0.20f;     // bottom 20%
    [Tooltip("Prevents rapid flicker when hovering around thresholds.")]
    [Range(0f, 0.2f)] public float hysteresis = 0.05f;

    [Header("Beat Matching")]
    [Tooltip("Max difference between watchedDot and beatDot (0..1 along bar) to count as 'in beat'.")]
    [Range(0f, 0.3f)] public float beatTolerance = 0.10f;
    [Tooltip("Minimum seconds between ammo grants.")]
    public float ammoCooldown = 0.25f;

    private enum Zone { Unknown, Top, Bottom }
    private Zone _zone = Zone.Unknown;

    // Edge detection for TOP in-beat only
    private bool _prevTopInBeat = false;
    private float _lastAmmoTime = -999f;

    void Update()
    {
        if (!barArea || !watchedDot || !beatDot) return;

        // Normalize dots to 0..1 along the bar
        float t = UITo01(barArea, watchedDot);
        float beatT = UITo01(barArea, beatDot);

        // --- Equip model switch with hysteresis ---
        Zone nextZone = ClassifyTwoState(t, _zone, topThreshold, bottomThreshold, hysteresis);
        if (nextZone != _zone && nextZone != Zone.Unknown)
        {
            _zone = nextZone;
            ApplyZone(_zone);
        }

        // --- TOP in-beat detection (one ammo per top pass) ---
        bool controllerAtTop = t >= topThreshold;
        bool beatAtTop = beatT >= topThreshold;
        bool closeEnough = Mathf.Abs(t - beatT) <= beatTolerance;

        bool topInBeat = controllerAtTop && beatAtTop && closeEnough;

        // Rising edge → grant ammo
        if (topInBeat && !_prevTopInBeat && Time.time >= _lastAmmoTime + ammoCooldown)
        {
            GrantAmmo(1);
            _lastAmmoTime = Time.time;
        }

        _prevTopInBeat = topInBeat;
    }

    private static float UITo01(RectTransform area, RectTransform dot)
    {
        float halfH = area.rect.height * 0.5f;
        return Mathf.InverseLerp(-halfH, +halfH, dot.anchoredPosition.y);
    }

    private static Zone ClassifyTwoState(float t, Zone current, float top, float bottom, float hys)
    {
        switch (current)
        {
            case Zone.Top:
                if (t < top - hys) return (t <= bottom) ? Zone.Bottom : Zone.Top;
                return Zone.Top;

            case Zone.Bottom:
                if (t > bottom + hys) return (t >= top) ? Zone.Top : Zone.Bottom;
                return Zone.Bottom;

            default: // Unknown
                if (t >= top) return Zone.Top;
                if (t <= bottom) return Zone.Bottom;
                return Zone.Unknown;
        }
    }

    private void ApplyZone(Zone z)
    {
        bool showTop = (z == Zone.Top);
        if (topVariant) topVariant.SetActive(showTop);
        if (bottomVariant) bottomVariant.SetActive(!showTop);
    }

    private void GrantAmmo(int amount)
    {
        if (!dataManager)
        {
            Debug.LogWarning("[HandItemChanger] No DataManager assigned. (+ammo skipped)");
            return;
        }

        // Prefer a method if you have one:
        // public void AddAmmo(int n) { ... }
        try
        {
            // If DataManager has AddAmmo(int), call it.
            var mi = dataManager.GetType().GetMethod("AddAmmo", new[] { typeof(int) });
            if (mi != null)
            {
                mi.Invoke(dataManager, new object[] { amount });
                return;
            }
        }
        catch { /* ignore and fall back */ }

        // Otherwise, try a public int ammo field/property
        var fi = dataManager.GetType().GetField("ammo");
        if (fi != null && fi.FieldType == typeof(int))
        {
            int current = (int)fi.GetValue(dataManager);
            fi.SetValue(dataManager, current + amount);
            return;
        }

        var pi = dataManager.GetType().GetProperty("ammo");
        if (pi != null && pi.CanRead && pi.CanWrite && pi.PropertyType == typeof(int))
        {
            int current = (int)pi.GetValue(dataManager);
            pi.SetValue(dataManager, current + amount);
            return;
        }

        Debug.LogWarning("[HandItemChanger] DataManager has neither AddAmmo(int) nor an int 'ammo'.");
    }
}
