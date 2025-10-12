using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class GameStatsSub : MonoBehaviour
{
    [SerializeField]
    private GameStatsPub gameStatsPub;

    [SerializeField]
    private TMP_Text scoreText;

    [SerializeField]
    private TMP_Text multiplierText;

    [SerializeField]
    private TMP_Text repText;

    [SerializeField]
    private TMP_Text repsText;

    [Header("Fail Colour Settings")]
    [SerializeField]
    private Color failColour;

    [SerializeField]
    private float failDuration;

    private Color originalColour;

    private void Awake()
    {
        originalColour = scoreText.color;
    }

    private void OnEnable()
    {
        gameStatsPub.OnScoreChanged += UpdateScore;
        gameStatsPub.OnMultiplierChanged += UpdateMultiplier;
        gameStatsPub.OnRepsChanged += UpdateReps;
        gameStatsPub.OnFailedRepEvent += FlashAllFailColours;

        UpdateScore(gameStatsPub.score);
        UpdateMultiplier(gameStatsPub.multiplier);
        UpdateReps(gameStatsPub.reps);
    }

    private void UpdateScore(float score)
    {
        float roundedScore = (float)Math.Round(score, 2);

        scoreText.text = $"${roundedScore}";
    }

    private void UpdateMultiplier(float multiplier)
    {
        float roundedMultipler = (float)Math.Round(multiplier, 1);

        multiplierText.text = $"{roundedMultipler} x";
    }

    private void UpdateReps(int reps)
    {
        repText.text = $"{reps}";
    }

    private void FlashAllFailColours()
    {
        StartCoroutine(FlashFailColour(scoreText));
        StartCoroutine(FlashFailColour(multiplierText));
        StartCoroutine(FlashFailColour(repsText));
        StartCoroutine(FlashFailColour(repText));
    }

    private IEnumerator FlashFailColour(TMP_Text text)
    {
        text.color = failColour;

        yield return new WaitForSeconds(failDuration);

        text.color = originalColour;
    }
}
