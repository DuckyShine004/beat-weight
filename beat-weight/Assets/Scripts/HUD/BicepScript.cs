using UnityEngine;

public class CurlHUDAnimator : MonoBehaviour
{
    [Header("Animator")]
    public Animator animator;
    public string stateName = "New Animation";
    public string subStatePath = ""; // e.g. "Arm" or "UpperBody/Arm"

    [Header("Tracked points")]
    public Transform shoulder;            // assign from your rig
    public Transform elbow;               // optional (if you have it)
    public Transform wristOrController;   // controller or hand

    [Header("Calibration (angle OR distance)")]
    public float extendAngleDeg = 170f;   // straight arm angle
    public float curlAngleDeg   =  50f;   // fully curled angle
    public float extendedDist   = 0.55f;  // meters shoulder→wrist when extended
    public float curledDist     = 0.15f;  // meters shoulder→wrist when curled

    [Range(0f, 0.2f)] public float smoothTime = 0.05f;

    int _fullPathHash;
    float _prog, _vel;

    void Awake()
    {
        if (!animator) animator = GetComponent<Animator>();
        if (!animator || !animator.runtimeAnimatorController)
        {
            Debug.LogError("Animator/controller missing.");
            enabled = false; return;
        }

        string fullPath = string.IsNullOrEmpty(subStatePath)
            ? $"Base Layer.{stateName}"
            : $"Base Layer.{subStatePath}.{stateName}";
        _fullPathHash = Animator.StringToHash(fullPath);

        if (!animator.HasState(0, _fullPathHash))
        {
            Debug.LogError($"State not found: {fullPath} on {animator.runtimeAnimatorController.name}");
            enabled = false; return;
        }

        animator.updateMode = AnimatorUpdateMode.UnscaledTime;   // works even if timeScale=0
        animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
        animator.speed = 0f;                                     // we scrub normalized time manually
    }

    [Header("Debug")]
public bool logDebug = true;

void Update()
{
    string why;
    float target = ComputeProgress01(out why);
    _prog = Mathf.SmoothDamp(_prog, target, ref _vel, smoothTime);

    if (logDebug)
        Debug.Log($"curl method={why}  target={target:0.000}  prog={_prog:0.000}");

    animator.Play(_fullPathHash, 0, _prog);
    animator.Update(0f);
}

[Header("Vertical (local) calibration")]
public float yExtended = 0.00f;  // wrist height above shoulder when arm straight
public float yCurled   = 0.25f;  // wrist height above shoulder when fully curled

float ComputeProgress01()
{
    // Controller position in shoulder's local space
    Vector3 local = shoulder.InverseTransformPoint(wristOrController.position);

    // Map vertical height to 0..1 (handles yExtended > yCurled or vice versa)
    float t = (local.y - yExtended) / Mathf.Max(1e-6f, (yCurled - yExtended));
    return Mathf.Clamp01(t);
}

static float Remap01(float a, float b, float v)
{
    // Works whether a<b or a>b, clamps to [0,1]
    if (Mathf.Abs(b - a) < 1e-6f) return 0f;
    float t = (v - a) / (b - a);
    return Mathf.Clamp01(t);
}

    // Call these once during a short calibration (player straightens, then fully curls)
    public void CalibrateExtended()
    {
        if (elbow)
        {
            float ang = Vector3.Angle(elbow.position - shoulder.position,
                                      wristOrController.position - elbow.position);
            extendAngleDeg = ang;
        }
        else extendedDist = Vector3.Distance(shoulder.position, wristOrController.position);
    }
    public void CalibrateCurled()
    {
        if (elbow)
        {
            float ang = Vector3.Angle(elbow.position - shoulder.position,
                                      wristOrController.position - elbow.position);
            curlAngleDeg = ang;
        }
        else curledDist = Vector3.Distance(shoulder.position, wristOrController.position);
    }
}