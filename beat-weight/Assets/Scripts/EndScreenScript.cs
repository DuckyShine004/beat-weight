using System;
using TMPro;
using UnityEngine;

public class EndScreenScript : MonoBehaviour
{
    [SerializeField]
    private GameStatsPub gameStatsPub;

    [SerializeField]
    private TMP_Text scoreText;

    [SerializeField]
    private TMP_Text songText;

    [SerializeField]
    private TMP_Text repsText;

    public String songName;
    private void OnEnable()
    {
        gameStatsPub.OnScoreChanged += UpdateScore;
        gameStatsPub.OnRepsChanged += UpdateReps;

        songText.text = songName;
        UpdateScore(gameStatsPub.score);
        UpdateReps(gameStatsPub.reps);
    }

    private void UpdateScore(float score)
    {
        float roundedScore = (float)Math.Round(score, 2);

        scoreText.text = $"${roundedScore}";
    }

    private void UpdateReps(int reps)
    {
        repsText.text = $"{reps}";
    }
}
