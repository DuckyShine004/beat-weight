using UnityEngine;
using TMPro;
using UnityEngine.SocialPlatforms.Impl;

public class ScoreTracking : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public DataManager dataManager;

    [Header("Popup Settings")]
    public TextMeshProUGUI popupPrefab;   // assign your "ScorePopup" prefab
    public Transform popupParent;         // usually the same canvas or near scoreText
    public float popupDuration = 1f;      // how long it lasts
    public float popupMoveY = 30f;        // how far it floats up

    public float speed = 5f; // speed of score change animation

    private int lastScore;
    private float displayedScore = 0f;

    void Start()
    {
        if (dataManager != null)
        {
            lastScore = dataManager.score;
            displayedScore = lastScore;
        }
    }

    void Update()
    {
        if (dataManager == null || scoreText == null) return;

        // Update score display
        displayedScore = Mathf.MoveTowards(displayedScore, dataManager.score, speed * Time.deltaTime);

        scoreText.text = "Score: " + Mathf.RoundToInt(displayedScore).ToString();


        // Detect change
        int diff = dataManager.score - lastScore;
        if (diff != 0)
        {
            ShowPopup(diff);
            lastScore = dataManager.score;
        }
    }

    void ShowPopup(int change)
    {
        if (!popupPrefab) return;

        // Create popup
        TextMeshProUGUI popup = Instantiate(popupPrefab, popupParent);
        popup.gameObject.SetActive(true);

        popup.text = (change > 0 ? "+" : "") + change.ToString();
        popup.color = change > 0 ? Color.green : Color.red;

        // Run floating animation
        StartCoroutine(FadeAndFloat(popup));
    }

    System.Collections.IEnumerator FadeAndFloat(TextMeshProUGUI popup)
    {
        float t = 0;
        Vector3 startPos = popup.rectTransform.localPosition;
        Color startColor = popup.color;

        while (t < popupDuration)
        {
            t += Time.deltaTime;
            float normalized = t / popupDuration;

            // move upward
            popup.rectTransform.localPosition = startPos + Vector3.up * (popupMoveY * normalized);

            // fade out
            popup.color = new Color(startColor.r, startColor.g, startColor.b, 1f - normalized);

            yield return null;
        }

        Destroy(popup.gameObject);
    }
}
