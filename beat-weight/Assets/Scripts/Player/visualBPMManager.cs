using UnityEngine;

public class VisualBPMManager : MonoBehaviour
{
    [System.Serializable]
    public class HandConfig
    {
        [Header("Input/Equip")]
        public RectTransform handDot; // controller dot for THIS hand
        public Transform handAnchor; // equip target for THIS hand
        public GameObject topVariant; // shown when in TOP zone for THIS hand
        public GameObject bottomVariant; // shown when in BOTTOM zone for THIS hand
    }

    [Header("Hand Selection")]
    public HandManager handManager; // assign your HandManager
    public HandConfig left;
    public HandConfig right;

    private RectTransform watchedDot; // the dot to watch (should match hand)

    [Header("UI (Vertical Bar)")]
    public RectTransform barArea;
    public RectTransform beatDot; // animated BPM dot

    [Header("BPM Settings")]
    public float bpm = 120f;
    public float offset = 0f; // seconds
    public bool visualAdjusted = false; // avoid clipping by subtracting dot height

    [Header("Beat Shape (in beats)")]
    public float beatUp = 2f; // example: 2 up + 2 down = 4 beats per cycle
    public float beatDown = 2f;
    public float holdbeatsTop = 0f;
    public float holdbeatsBottom = 0f;

    private Transform handAnchor;
    private GameObject topVariant;
    private GameObject bottomVariant;

    [Header("Thresholds")]
    [Range(0f, 1f)]
    public float topThreshold = 0.80f;

    [Range(0f, 1f)]
    public float bottomThreshold = 0.20f;

    [Range(0f, 0.2f)]
    public float hysteresis = 0.05f;

    [Header("Beat Matching (for ammo/top logic)")]
    [Range(0f, 0.3f)]
    public float beatTolerance = 0.10f;
    public float ammoCooldown = 0.25f;

    [Header("SCORING MODE")]
    public bool usePerBeatScoring = true; // enable per-beat pulses

    [Header("Continuous Scoring")]
    [Range(0.01f, 1f)]
    public float scoreWindow = 0.12f; // distance >= window → 0
    public float scoreRateAtPerfect = 10f; // pts/sec at perfect
    public AnimationCurve scoreCurve = AnimationCurve.EaseInOut(0, 0f, 1, 1f);

    [Range(0.01f, 1f)]
    public float perBeatWindow = 0.12f; // closeness window for awarding

    [Tooltip("If true, only award on beats when both the hand and beat are in the TOP zone.")]
    public bool requireTopForPerBeat = false;

    [Header("Game/Data")]
    public DataManager dataManager; // expects AddScore(float) or score field/property; AddAmmo(int) optional
    private float watchedPosition;

    [Header("Game Stats")]
    public GameStatsPub gameStatsPub;

    [Header("Debugging")]
    public bool isDebugging;

    // --- internal state ---
    private enum Zone
    {
        Unknown,
        Top,
        Bottom,
    }

    private Zone _zone = Zone.Unknown;
    private float _timer = 0f;
    private int _lastBeatIndex = -1; // NEW: per-beat pulse tracker
    private int numberOfSyncedBeats;
    private bool shootInPreviousRep;

    void Start()
    {
        Reset();
    }

    void OnEnable()
    {
        Reset();
    }

    void Reset()
    {
        _lastBeatIndex = -1;
        _zone = Zone.Unknown;
        numberOfSyncedBeats = 0;
        shootInPreviousRep = false;
        _timer = 0f;

        watchedPosition = 0f;
        if (handManager)
        {
            if (handManager.activeHand == HandManager.Hand.Left)
            {
                watchedDot = left.handDot;
                handAnchor = left.handAnchor;
                topVariant = left.topVariant;
                bottomVariant = left.bottomVariant;
            }
            else
            {
                watchedDot = right.handDot;
                handAnchor = right.handAnchor;
                topVariant = right.topVariant;
                bottomVariant = right.bottomVariant;
            }
        }
    }

    // Debugging information (TURN OFF IN FINAL)
    void OnGUI()
    {
        if (!isDebugging)
        {
            return;
        }

        GUI.matrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);

        GUILayout.BeginArea(new Rect(60, 60, 200, 200), GUI.skin.box);

        string title = "<b>Visual BPM Manager</b>";

        GUILayout.Label(title, new GUIStyle(GUI.skin.label) { richText = true });

        GUILayout.Label($"Number of synced beats: {numberOfSyncedBeats}");
        GUILayout.Label($"Shoot in previous rep: {shootInPreviousRep}");

        GUILayout.EndArea();
    }

    void Update()
    {
        if (!barArea || !watchedDot || !beatDot || bpm <= 0f)
        {
            return;
        }

        // 1) Animate beatDot along the bar
        float beatT = ComputeBeat01();
        ApplyDotY(beatDot, beatT);

        // 2) Normalized positions (0..1)
        float tWatched = UITo01(barArea, watchedDot);
        float tBeat = UITo01(barArea, beatDot);
        watchedPosition = tWatched;

        // 3) Equip zone toggle with hysteresis
        Zone nextZone = ClassifyTwoState(
            tWatched,
            _zone,
            topThreshold,
            bottomThreshold,
            hysteresis
        );
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

        // Calculating if we are at the bottom beat, bruh use this
        float beatInterval = 60f / bpm;
        float timeSinceStart = _timer + offset;
        int currentBeatIndex = Mathf.FloorToInt((timeSinceStart + 1e-4f) / beatInterval);

        bool atBottom = false;
        bool isNewBeat = currentBeatIndex > _lastBeatIndex;

        // Check if beat is at the bottom
        if (isNewBeat && beatAtBottom)
        {
            if (!shootInPreviousRep)
            {
                gameStatsPub.OnFailedRep();
            }

            numberOfSyncedBeats = 0;
            atBottom = true;
        }

        // 6) PER-BEAT scoring (fires once each beat)
        if (usePerBeatScoring)
        {
            if (isNewBeat)
            {
                // A new beat just occurred → evaluate and award if close
                bool passTopGate = !requireTopForPerBeat || (controllerAtTop && beatAtTop);

                float dist = Mathf.Abs(tWatched - tBeat);

                if (passTopGate && dist <= perBeatWindow)
                {
                    gameStatsPub.OnBeatSync();

                    // Only increment if not at bottom since beat resets at bottom
                    if (!atBottom)
                    {
                        ++numberOfSyncedBeats;
                    }
                }

                _lastBeatIndex = currentBeatIndex;
            }
        }

        // Reset at bottom or at top? Up to you- currently resetting at bottom
        if (isNewBeat && beatAtTop)
        {
            if (topInBeat && numberOfSyncedBeats >= 4)
            {
                ShootGun();
                shootInPreviousRep = true;
            }
            else
            {
                shootInPreviousRep = false;
            }
        }

        // 7) advance local time
        _timer += Time.deltaTime;
    }

    public float GetWatchedPosition()
    {
        return watchedPosition;
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

        if (t < upDur) // up ramp
            return t / upDur;
        t -= upDur;

        if (t < topHold) // hold top
            return 1f;
        t -= topHold;

        if (t < downDur) // down ramp
            return 1f - (t / downDur);
        // hold bottom
        return 0f;
    }

    private void ApplyDotY(RectTransform dot, float t01)
    {
        float h = barArea.rect.height;
        if (visualAdjusted && dot)
            h -= dot.rect.height;
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
                if (t < top - hys)
                    return (t <= bottom) ? Zone.Bottom : Zone.Top;
                return Zone.Top;
            case Zone.Bottom:
                if (t > bottom + hys)
                    return (t >= top) ? Zone.Top : Zone.Bottom;
                return Zone.Bottom;
            default:
                if (t >= top)
                    return Zone.Top;
                if (t <= bottom)
                    return Zone.Bottom;
                return Zone.Unknown;
        }
    }

    private void ApplyZone(Zone z)
    {
        bool showTop = (z == Zone.Top);
        if (topVariant)
            topVariant.SetActive(showTop);
        if (bottomVariant)
            bottomVariant.SetActive(!showTop);
    }

    // ---- Data hooks (no changes to DataManager required) ----
    private void ShootGun()
    {
        VRTriggerShoot shooter = GetComponent<VRTriggerShoot>();
        // Negative z direction is forward
        if (shooter)
            shooter.ShootWorldDir(new Vector3(0, 0, -1));
    }
}
