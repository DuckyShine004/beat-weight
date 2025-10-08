using UnityEngine;
using System.Collections.Generic;



public class FollowCurveBPM : MonoBehaviour
{
    [Header("Curve and Movement")]
    public BezierCurve curve;
    public Transform objectToMove;
    public BeatHandSyncController handSyncController;
    [Range(30f, 240f)] public float bpm = 120f;   // beats per minute
    [Min(0f)] public float delay = 0f;

    [Header("Timing (beats per leg)")]
    [Min(0.01f)] public float beatsUp = 4f;       // beats to go 0 -> 1
    [Min(0.01f)] public float beatsDown = 4f;     // beats to go 1 -> 0

    public Animator animation;
    public int maxreps = 0; // 0 = infinite
    private int currentrep = 0;

    [Header("Motion")]
    public bool smoothMotion = true;              // ease in/out

    private enum Leg { Up, Down }
    private Leg currentLeg = Leg.Up;

    private float legElapsed = 0f;  // seconds elapsed in current leg
    private float legDuration = 0f; // seconds for current leg

    void OnEnable()
    {
        ResetLeg(Leg.Up);
        currentrep = 0;
        legElapsed = 0f;
    }

    void Update()
    {
        if (delay > 0f)
        {
            delay -= Time.deltaTime;
            return;
        }
        if (!curve) return;


        // If BPM changes at runtime, keep duration consistent:
        float expected = SecondsPerBeat() * GetBeatsFor(currentLeg);
        if (!Mathf.Approximately(expected, legDuration))
            legDuration = expected;

        legElapsed += Time.deltaTime;
        float legT = Mathf.Clamp01(legElapsed / legDuration); // 0..1 within the current leg

        // Map leg progress to curve t:
        float t = (currentLeg == Leg.Up) ? legT : (1f - legT);
        float tSmooth = smoothMotion ? Mathf.SmoothStep(0f, 1f, t) : t;

        objectToMove.position = curve.GetPoint(tSmooth);
        if(tSmooth==0f) {
            animation.SetTrigger("play");
            print("pulse");
            animation.SetTrigger("stop");
        }

        // Leg finished? swap legs
        if (legT >= 1f - Mathf.Epsilon)
        {
            currentLeg = (currentLeg == Leg.Up) ? Leg.Down : Leg.Up;
            if (currentLeg == Leg.Up)
            {
                currentrep++;
                if (maxreps > 0 && currentrep >= maxreps)
                {
                    enabled = false;
                    handSyncController.enabled = false;
                    return;
                }
            }
            ResetLeg(currentLeg);
        }
    }

    private void ResetLeg(Leg leg)
    {
        currentLeg = leg;
        legElapsed = 0f;
        legDuration = SecondsPerBeat() * GetBeatsFor(leg);
    }

    private float SecondsPerBeat() => 60f / Mathf.Max(1f, bpm);
    private float GetBeatsFor(Leg leg) => (leg == Leg.Up) ? beatsUp : beatsDown;
}
