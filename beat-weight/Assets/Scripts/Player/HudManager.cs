using UnityEngine;

public class ControllerHeightsToDots : MonoBehaviour
{
    [Header("Controller visuals (world-space)")]
    public Transform leftControllerVisual;      // optional
    public Transform rightControllerVisual;     // optional

    [Header("UI")]
    public RectTransform barArea;               // vertical track (parent)
    public RectTransform leftDot;               // optional (acts as indicator)
    public RectTransform rightDot;              // optional (acts as indicator)

    [Header("Hotkeys")]
    public KeyCode setTopKey = KeyCode.T;       // set TOP height
    public KeyCode setBottomKey = KeyCode.B;    // set BOTTOM height

    // Manually-set extremes via T/B
    private float _min = float.PositiveInfinity;
    private float _max = float.NegativeInfinity;

    void Update()
    {
        if (!barArea) return;

        // Hotkeys
        if (Input.GetKeyDown(setTopKey)) SetTopToCurrent();
        if (Input.GetKeyDown(setBottomKey)) SetBottomToCurrent();

        if (leftDot && leftControllerVisual) SetDotY(leftDot, leftControllerVisual.position.y);
        if (rightDot && rightControllerVisual) SetDotY(rightDot, rightControllerVisual.position.y);
    }

    // ----- Calibration -----
    public void SetTopToCurrent()
    {
        if (leftControllerVisual)
            _max = leftControllerVisual.position.y;
    }

    public void SetBottomToCurrent()
    {
        if (leftControllerVisual)
            _min = leftControllerVisual.position.y;
    }

    [ContextMenu("Reset Calibration")]
    public void ResetCalibration()
    {
        _min = float.PositiveInfinity;
        _max = float.NegativeInfinity;
    }

    private void FixOrderIfNeeded()
    {
        if (IsFinite(_min) && IsFinite(_max) && _min > _max)
        {
            float tmp = _min; _min = _max; _max = tmp;
        }
    }

    private static bool IsFinite(float f) => !float.IsNaN(f) && !float.IsInfinity(f);

    private void SetDotY(RectTransform dot, float controllerWorldY)
    {
        if (!IsFinite(_min) || !IsFinite(_max))
        {
            var p0 = dot.anchoredPosition;
            p0.y = 0f;
            dot.anchoredPosition = p0;
            return;
        }

        // Normalize world meters -> 0..1 based on calibrated min/max
        float t = Mathf.Clamp01(Mathf.InverseLerp(_min, _max, controllerWorldY));

        // Map 0..1 to barArea's pixel height
        float halfH = barArea.rect.height * 0.5f;
        float yLocal = Mathf.Lerp(-halfH, +halfH, t);

        var p = dot.anchoredPosition;
        p.y = yLocal;                   // dot MUST be a child of barArea
        dot.anchoredPosition = p;
    }
}
