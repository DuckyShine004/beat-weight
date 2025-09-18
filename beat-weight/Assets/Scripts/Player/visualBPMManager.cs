using UnityEngine;

public class visualBPMManager : MonoBehaviour
{
    [Header("UI")]
    public RectTransform barArea;
    public RectTransform dot;

    [Header("BPM Settings")]
    public float bpm = 120f;
    public float offset = 0f; // Offset in seconds
    [Header("Beat Settings")]
    public float beatUp = 1f; // Number of beats to move up
    public float beatDown = 1f; // Number of beats to move down
    public float holdbeatsTop = 0f; // Number of beats to hold the position
    public float holdbeatsBottom = 0f; // Number of beats to hold the position

    public float timer = 0f;

    public bool visualAdjusted = false;




    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (bpm <= 0 || barArea == null || dot == null) return;

        float beatInterval = 60f / bpm; // Time for one beat in seconds
        float timeSinceStart = timer + offset; // Adjusted time with offset
        float totalBeats = beatUp + beatDown + holdbeatsTop + holdbeatsBottom;
        float currentBeatTime = timeSinceStart % (totalBeats * beatInterval);
        float beatPosition = 0f;
        if (currentBeatTime < beatUp * beatInterval)
        {
            // Moving up
            beatPosition = currentBeatTime / (beatUp * beatInterval);
        }
        else if (currentBeatTime < (beatUp + holdbeatsTop) * beatInterval)
        {
            // Holding at the top
            beatPosition = 1f;
        }
        else if (currentBeatTime < (beatUp + holdbeatsTop + beatDown) * beatInterval)
        {
            // Moving down
            float downTime = currentBeatTime - (beatUp + holdbeatsTop) * beatInterval;
            beatPosition = 1f - (downTime / (beatDown * beatInterval));
        }
        else
        {
            // Holding at the bottom
            beatPosition = 0f;
        }

        float barHeight = barArea.rect.height;
        if (visualAdjusted)
            barHeight -= dot.rect.height; // Adjust for dot height
        float dotYPosition = dot.anchoredPosition.y;
        dotYPosition = Mathf.Lerp(-barHeight / 2, barHeight / 2, beatPosition);
        dot.anchoredPosition = new Vector2(dot.anchoredPosition.x, dotYPosition);
        timer += Time.deltaTime;
    }
}
