using UnityEngine;

public class ControllerHeightsToDotsSimple : MonoBehaviour
{
    [Header("Controller visuals (world-space)")]
    public Transform leftControllerVisual;    // optional
    public Transform rightControllerVisual;   // optional

    [Header("UI")]
    public RectTransform barArea;             // vertical track (parent)
    public RectTransform leftDot;             // optional
    public RectTransform rightDot;            // optional

    public KeyCode resetKey = KeyCode.R;

    [Header("Smoothing")]
    [Range(0f, 30f)] public float smoothLerpPerSec = 10f;

    // Shared extremes across both hands
    private float _min = float.PositiveInfinity;
    private float _max = float.NegativeInfinity;

    // Smoothed positions (0..1)
    private float _tL, _tR;

    void Update()
    {
        if (!barArea) return;

        // Read current heights (if present)
        bool hasL = leftControllerVisual;
        bool hasR = rightControllerVisual;

        float yL = hasL ? leftControllerVisual.position.y : 0f;
        float yR = hasR ? rightControllerVisual.position.y : 0f;

        // Update shared min/max from whichever controllers are present
        if (hasL) { if (yL < _min) _min = yL; if (yL > _max) _max = yL; }
        if (hasR) { if (yR < _min) _min = yR; if (yR > _max) _max = yR; }

        float halfH = barArea.rect.height * 0.5f;

        if (hasL && leftDot)
        {
            float t = SafeInverseLerp(_min, _max, yL);
            _tL = Smooth(_tL, t, smoothLerpPerSec);
            SetDotY(leftDot, Mathf.Lerp(-halfH, +halfH, _tL));
        }

        if (hasR && rightDot)
        {
            float t = SafeInverseLerp(_min, _max, yR);
            _tR = Smooth(_tR, t, smoothLerpPerSec);
            SetDotY(rightDot, Mathf.Lerp(-halfH, +halfH, _tR));
        }

        if (Input.GetKeyDown(resetKey))
            ResetCalibration();
    }

    private static float Smooth(float current, float target, float perSec) =>
        perSec > 0f ? Mathf.Lerp(current, target, 1f - Mathf.Exp(-perSec * Time.deltaTime)) : target;

    private static float SafeInverseLerp(float min, float max, float v)
    {
        if (!IsFinite(min) || !IsFinite(max) || Mathf.Abs(max - min) < 1e-4f) return 0.5f; // center until we have spread
        return Mathf.Clamp01(Mathf.InverseLerp(min, max, v));
    }

    private static bool IsFinite(float f) => !float.IsNaN(f) && !float.IsInfinity(f);

    private static void SetDotY(RectTransform dot, float yLocal)
    {
        var p = dot.anchoredPosition;
        p.y = yLocal;                // dot should be a CHILD of barArea
        dot.anchoredPosition = p;
    }

    // Optional helper you can call from Inspector context menu
    [ContextMenu("Reset Calibration")]
    public void ResetCalibration()
    {
        _min = float.PositiveInfinity;
        _max = float.NegativeInfinity;
    }
}
