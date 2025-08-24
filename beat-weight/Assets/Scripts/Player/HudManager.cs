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

        // Move any assigned dots to same Y
        float halfH = barArea.rect.height * 0.5f;
        float y = 0f;
        if (leftDot) SetDotY(leftDot, y);
        if (rightDot) SetDotY(rightDot, y);
    }

    // ----- Calibration -----
    public void SetTopToCurrent()
    {
        if (leftControllerVisual)
            _max = leftControllerVisual.position.y + leftDot.rect.height;
    }

    public void SetBottomToCurrent()
    {
        if (leftControllerVisual)
            _min = leftControllerVisual.position.y - leftDot.rect.height;
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

    private static void SetDotY(RectTransform dot, float yLocal)
    {
        var p = dot.anchoredPosition;
        p.y = yLocal;
        dot.anchoredPosition = p;
    }
}
