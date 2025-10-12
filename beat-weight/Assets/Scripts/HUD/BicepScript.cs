using UnityEngine;

public class CurlHUDAnimator : MonoBehaviour
{
    [Header("Animator")]
    public Animator animator;
    public string stateName = "New Animation";

    [Header("Tracked points")]
    public Transform shoulder;
    public Transform wristOrController;
    public Collider2D sliderCollider; // Optional: if you want to enable/disable a slider collider
    public UnityEngine.UI.Image BicepImage;
    public Color ActiveColor;


    [Header("Vertical calibration (local Y relative to shoulder)")]
    public float yExtended = 0.00f;  // wrist height above shoulder when arm straight
    public float yCurled = 0.25f;  // wrist height above shoulder when fully curled

    [Range(0f, 0.2f)] public float smoothTime = 0.05f;
    public bool logDebug = false;

    int _fullPathHash;
    float _prog, _vel;

    enum Half { Up, Down }
    [Header("Direction gating")]
    public float velThreshold = 0.05f;   // m/s needed to switch direction (tune)

    Half _half = Half.Up;
    Half _prevHalf = Half.Up;
    float _nt = 0f;          // normalized time we drive (0..1)
    float _lastLocalY = 0f;

    void Start()
    {
        // Initialize lastLocalY from current pose
        _lastLocalY = shoulder.InverseTransformPoint(wristOrController.position).y;
        ActiveColor.a = 1;
    }

    void Update()
    {
        // Get wrist height relative to shoulder (local Y) and velocity
        Vector3 local = shoulder.InverseTransformPoint(wristOrController.position);
        float y = local.y;
        float dt = Mathf.Max(Time.unscaledDeltaTime, 1e-4f);
        float vy = (y - _lastLocalY) / dt;
        _lastLocalY = y;

        // Direction gating with hysteresis
        if (vy > velThreshold) _half = Half.Up;
        else if (vy < -velThreshold) _half = Half.Down;

        if (_half != _prevHalf)
        {
            // When we switch halves, snap to the start of that half
            if (_half == Half.Up) _nt = 0f;   // restart from bottom half
            else _nt = 0.5f; // start at top for the down half
            _prevHalf = _half;
        }

        // Map current height to the correct half of the clip
        float p = Remap01(yExtended, yCurled, y);  // 0=bottom, 1=top (based on your calibration)

        float ntCandidate = (_half == Half.Up)
            ? p * 0.5f                 // 0..0.5
            : 0.5f + (1f - p) * 0.5f;  // 0.5..1, as hand lowers

        // Make time monotonic within each half and clamp to the end of that half
        if (ntCandidate > _nt) _nt = ntCandidate;
        if (_half == Half.Up) _nt = Mathf.Min(_nt, 0.5f); // stop at 50% (the top)
        else _nt = Mathf.Min(_nt, 1.0f); // stop at 100% (the bottom)

        // Drive the Animator
        animator.Play(_fullPathHash, 0, _nt);
        animator.Update(0f);

        if (logDebug)
            Debug.Log($"half={_half} vy={vy:F3} y={y:F3} p={p:F2} nt={_nt:F3}");

    }

    static float Remap01(float a, float b, float v)
    {
        if (Mathf.Abs(b - a) < 1e-6f) return 0f;
        return Mathf.Clamp01((v - a) / (b - a));
    }

    // void OnTriggerEnter2D(Collider2D collider)
    // {

    //     BicepImage.color = ActiveColor;
    //     Debug.Log("Trigger Entered");
    // }

    // private void OnTriggerExit2D(Collider2D collision)
    // {
    //     Color c = ActiveColor;
    //     c.a = 0;
    //     BicepImage.color = c;
    // }
}