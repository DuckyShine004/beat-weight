using UnityEngine;

public class BeatHandSyncController : MonoBehaviour
{
    [Header("UI (Vertical Bar)")]
    public RectTransform barArea;
    public RectTransform watchedDot;   // controller dot
    public RectTransform beatDot;      // animated BPM dot

    [Header("BPM Settings")]
    public float bpm = 120f;
    public float offset = 0f;                  // seconds
    public bool visualAdjusted = false;        // avoid clipping by subtracting dot height

    [Header("Beat Shape (in beats)")]
    public float beatUp = 2f;                  // example: 2 up + 2 down = 4 beats per cycle
    public float beatDown = 2f;
    public float holdbeatsTop = 0f;
    public float holdbeatsBottom = 0f;

    [Header("Equip Targets")]
    public Transform handAnchor;
    public GameObject topVariant;
    public GameObject bottomVariant;

    [Header("Thresholds")]
    [Range(0f, 1f)] public float topThreshold = 0.80f;
    [Range(0f, 1f)] public float bottomThreshold = 0.20f;
    [Range(0f, 0.2f)] public float hysteresis = 0.05f;

    [Header("Beat Matching (for ammo/top logic)")]
    [Range(0f, 0.3f)] public float beatTolerance = 0.10f;
    public float ammoCooldown = 0.25f;

    [Header("SCORING MODE")]
    public bool useContinuousScoring = false;  // set false if you only want per-beat scoring
    public bool usePerBeatScoring = true;      // enable per-beat pulses

    [Header("Continuous Scoring")]
    [Range(0.01f, 1f)] public float scoreWindow = 0.12f; // distance >= window → 0
    public float scoreRateAtPerfect = 10f;               // pts/sec at perfect
    public AnimationCurve scoreCurve = AnimationCurve.EaseInOut(0, 0f, 1, 1f);

    [Header("Per-Beat Scoring")]
    public float pointsPerBeat = 5f;                      // points if within window at the beat tick
    [Range(0.01f, 1f)] public float perBeatWindow = 0.12f;// closeness window for awarding
    [Tooltip("If true, only award on beats when both the hand and beat are in the TOP zone.")]
    public bool requireTopForPerBeat = false;

    [Header("Score Ammo Thresholds")]
    public float scoreThresholdForAmmo = 15f;             // award ammo when this much

    [Header("Game/Data")]
    public DataManager dataManager;   // expects AddScore(float) or score field/property; AddAmmo(int) optional

    // --- internal state ---
    private enum Zone { Unknown, Top, Bottom }
    private Zone _zone = Zone.Unknown;
    private bool _prevTopInBeat = false;
    private float _lastAmmoTime = -999f;
    private float _timer = 0f;
    private int _lastBeatIndex = -1;  // NEW: per-beat pulse tracker

    private float _scoreSinceLastAmmo = 0f;


    void Start() { _timer = 0f; }

    void Update()
    {
        if (!barArea || !watchedDot || !beatDot || bpm <= 0f) return;

        // 1) Animate beatDot along the bar
        float beatT = ComputeBeat01();
        ApplyDotY(beatDot, beatT);

        // 2) Normalized positions (0..1)
        float tWatched = UITo01(barArea, watchedDot);
        float tBeat = UITo01(barArea, beatDot);

        // 3) Equip zone toggle with hysteresis
        Zone nextZone = ClassifyTwoState(tWatched, _zone, topThreshold, bottomThreshold, hysteresis);
        if (nextZone != _zone && nextZone != Zone.Unknown)
        {
            _zone = nextZone;
            ApplyZone(_zone);
        }

        // 4) (Optional) Ammo on top in-beat rising edge
        bool controllerAtTop = tWatched >= topThreshold;
        bool beatAtTop = tBeat >= topThreshold;
        bool beatAtBottom = tBeat <= bottomThreshold;
        bool controllerAtBottom = tWatched <= bottomThreshold;
        bool closeEnough = Mathf.Abs(tWatched - tBeat) <= beatTolerance;
        bool topInBeat = controllerAtTop && beatAtTop && closeEnough;
        bool botInBeat = controllerAtBottom && beatAtBottom && closeEnough;

        if (beatAtBottom)
        {
            _scoreSinceLastAmmo = 0f; // reset score accumulator on each beat cycle
        }

        // 5) CONTINUOUS scoring (optional)
        if (useContinuousScoring)
        {
            float distance = Mathf.Abs(tWatched - tBeat);
            float nd = Mathf.Clamp01(distance / Mathf.Max(1e-5f, scoreWindow));
            float closeness = 1f - nd; // 1 at perfect
            float multiplier = Mathf.Clamp01(scoreCurve.Evaluate(closeness));
            float deltaScore = scoreRateAtPerfect * multiplier * Time.deltaTime;
            if (deltaScore > 0f) AddScore(deltaScore);
        }

        // 6) PER-BEAT scoring (fires once each beat)
        if (usePerBeatScoring)
        {
            float beatInterval = 60f / bpm;
            float timeSinceStart = _timer + offset;

            // robust index against float jitter
            int currentBeatIndex = Mathf.FloorToInt((timeSinceStart + 1e-4f) / beatInterval);
            if (currentBeatIndex > _lastBeatIndex)
            {
                // A new beat just occurred → evaluate and award if close
                bool passTopGate = !requireTopForPerBeat || (controllerAtTop && beatAtTop);
                float dist = Mathf.Abs(tWatched - tBeat);
                if (passTopGate && dist <= perBeatWindow)
                {
                    AddScore(pointsPerBeat);
                    _scoreSinceLastAmmo += pointsPerBeat;
                }
                _lastBeatIndex = currentBeatIndex;
            }
        }

        if (topInBeat && !_prevTopInBeat && Time.time >= _lastAmmoTime + ammoCooldown && _scoreSinceLastAmmo >= scoreThresholdForAmmo)
        {
            GrantAmmo(1);
            _lastAmmoTime = Time.time;
        }
        _prevTopInBeat = topInBeat;

        // 7) advance local time
        _timer += Time.deltaTime;
    }

    // ---- Beat animation (maps time -> 0..1 along bar) ----
    private float ComputeBeat01()
    {
        float beatInterval = 60f / bpm; // seconds per beat
        float totalBeats = Mathf.Max(beatUp + beatDown + holdbeatsTop + holdbeatsBottom, 0.0001f);
        float periodSec = totalBeats * beatInterval;

        float t = (_timer + offset) % periodSec;

        float upDur = beatUp * beatInterval;
        float topHold = holdbeatsTop * beatInterval;
        float downDur = beatDown * beatInterval;
        float botHold = holdbeatsBottom * beatInterval;

        if (t < upDur)                               // up ramp
            return t / upDur;
        t -= upDur;

        if (t < topHold)                             // hold top
            return 1f;
        t -= topHold;

        if (t < downDur)                             // down ramp
            return 1f - (t / downDur);
        // hold bottom
        return 0f;
    }

    private void ApplyDotY(RectTransform dot, float t01)
    {
        float h = barArea.rect.height;
        if (visualAdjusted && dot) h -= dot.rect.height;
        float y = Mathf.Lerp(-h * 0.5f, h * 0.5f, t01);
        var p = dot.anchoredPosition;
        dot.anchoredPosition = new Vector2(p.x, y);
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
            default:
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

    // ---- Data hooks (no changes to DataManager required) ----
    private void GrantAmmo(int amount)
    {
        if (!dataManager) return;
        try
        {
            var mi = dataManager.GetType().GetMethod("AddAmmo", new[] { typeof(int) });
            if (mi != null) { mi.Invoke(dataManager, new object[] { amount }); return; }
        }
        catch { }
        var fi = dataManager.GetType().GetField("ammo");
        if (fi != null && fi.FieldType == typeof(int)) { fi.SetValue(dataManager, (int)fi.GetValue(dataManager) + amount); return; }
        var pi = dataManager.GetType().GetProperty("ammo");
        if (pi != null && pi.CanRead && pi.CanWrite && pi.PropertyType == typeof(int))
            pi.SetValue(dataManager, (int)pi.GetValue(dataManager) + amount);
    }

    private void AddScore(float amount)
    {
        if (!dataManager) return;
        try
        {
            var miF = dataManager.GetType().GetMethod("AddScore", new[] { typeof(float) });
            if (miF != null) { miF.Invoke(dataManager, new object[] { amount }); return; }
            var miI = dataManager.GetType().GetMethod("AddScore", new[] { typeof(int) });
            if (miI != null) { miI.Invoke(dataManager, new object[] { Mathf.RoundToInt(amount) }); return; }
        }
        catch { }
        var fi = dataManager.GetType().GetField("score");
        if (fi != null)
        {
            if (fi.FieldType == typeof(float)) { fi.SetValue(dataManager, (float)fi.GetValue(dataManager) + amount); return; }
            if (fi.FieldType == typeof(int)) { fi.SetValue(dataManager, (int)fi.GetValue(dataManager) + Mathf.RoundToInt(amount)); return; }
        }
        var pi = dataManager.GetType().GetProperty("score");
        if (pi != null && pi.CanRead && pi.CanWrite)
        {
            if (pi.PropertyType == typeof(float)) { pi.SetValue(dataManager, (float)pi.GetValue(dataManager) + amount); return; }
            if (pi.PropertyType == typeof(int)) pi.SetValue(dataManager, (int)pi.GetValue(dataManager) + Mathf.RoundToInt(amount));
        }
    }
}
