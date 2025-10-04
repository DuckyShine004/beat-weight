using System;
using TMPro;
using UnityEngine;

public class GameStatsSub : MonoBehaviour
{
    [SerializeField]
    private GameStatsPub gameStatsPub;

    [SerializeField]
    private TMP_Text scoreText;

    [SerializeField]
    private TMP_Text multiplerText;

    [SerializeField]
    private TMP_Text repsText;

    private void OnEnable()
    {
        gameStatsPub.OnScoreChanged += UpdateScore;
        gameStatsPub.OnMultiplierChanged += UpdateMultiplier;
        gameStatsPub.OnRepsChanged += UpdateReps;

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

        multiplerText.text = $"{roundedMultipler} x";
    }

    private void UpdateReps(int reps)
    {
        repsText.text = $"{reps}";
    }
}
