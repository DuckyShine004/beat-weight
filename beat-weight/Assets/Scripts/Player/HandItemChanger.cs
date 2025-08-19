using UnityEngine;

public class HandItemChanger : MonoBehaviour
{
    [Header("UI")]
    public RectTransform barArea;     // vertical track (parent)
    public RectTransform watchedDot;  // the dot (child of barArea)

    [Header("Equip targets")]
    public Transform handAnchor;      // where to attach items (optional)
    public GameObject topVariant;     // active in top band
    public GameObject bottomVariant;  // active in bottom band

    [Header("Thresholds")]
    [Range(0f,1f)] public float topThreshold = 0.80f;     // top 20%
    [Range(0f,1f)] public float bottomThreshold = 0.20f;  // bottom 20%
    [Range(0f,0.2f)] public float hysteresis = 0.05f;     // buffer to prevent flapping

    private enum Zone { Unknown, Top, Bottom }
    private Zone _zone = Zone.Unknown;

    void Update()
    {
        if (!barArea || !watchedDot) return;

        // Map dot's anchored Y to 0..1 along the bar
        float halfH = barArea.rect.height * 0.5f;
        float t = Mathf.InverseLerp(-halfH, +halfH, watchedDot.anchoredPosition.y);

        Zone next = ClassifyTwoState(t, _zone);
        if (next != _zone)
        {
            _zone = next;
            ApplyZone(_zone);
        }
    }

    private Zone ClassifyTwoState(float t, Zone current)
    {
        // Enter Top when >= topThreshold; stay Top until below (topThreshold - hysteresis)
        // Enter Bottom when <= bottomThreshold; stay Bottom until above (bottomThreshold + hysteresis)
        // Otherwise (middle band), hold current state.

        switch (current)
        {
            case Zone.Top:
                if (t < topThreshold - hysteresis)
                    return (t <= bottomThreshold) ? Zone.Bottom : Zone.Top; // leave only if far enough down
                return Zone.Top;

            case Zone.Bottom:
                if (t > bottomThreshold + hysteresis)
                    return (t >= topThreshold) ? Zone.Top : Zone.Bottom; // leave only if far enough up
                return Zone.Bottom;

            default: // Unknown -> pick one if inside a band, else stay Unknown until a band is entered
                if (t >= topThreshold)    return Zone.Top;
                if (t <= bottomThreshold) return Zone.Bottom;
                return Zone.Unknown;
        }
    }

    private void ApplyZone(Zone z)
{
    bool topOn = (z == Zone.Top);
    if (topVariant)    topVariant.SetActive(topOn);
    if (bottomVariant) bottomVariant.SetActive(!topOn);
}

    [ContextMenu("Force Top")]
    public void ForceTop()  { _zone = Zone.Top;    ApplyZone(_zone); }
    [ContextMenu("Force Bottom")]
    public void ForceBot()  { _zone = Zone.Bottom; ApplyZone(_zone); }
}