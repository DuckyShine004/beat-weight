using System;
using UnityEngine;

public class GameStatsPub : MonoBehaviour
{
    [Header("Base game stats")]
    public float baseScore;
    public float baseMultiplier;

    public float score { get; private set; }
    public float multiplier { get; private set; }
    public int reps { get; private set; }

    public event Action<float> OnScoreChanged;
    public event Action<float> OnMultiplierChanged;
    public event Action<int> OnRepsChanged;

    public void Start()
    {
        SetScore(0.0f);
        SetMultiplier(1.0f);
        SetReps(0);
    }

    public void OnSuccessfulRep()
    {
        AddScore();

        IncrementMultiplier();
        IncrementReps();
    }

    public void OnFailedRep()
    {
        SetMultiplier(1.0f);
    }

    public void AddScore()
    {
        score += baseScore * multiplier;

        OnScoreChanged?.Invoke(score);
    }

    private void IncrementMultiplier()
    {
        multiplier += baseMultiplier;

        SetMultiplier(multiplier);
    }

    private void IncrementReps()
    {
        ++reps;

        SetReps(reps);
    }

    private void SetScore(float score)
    {
        this.score = score;

        OnScoreChanged?.Invoke(score);
    }

    private void SetMultiplier(float multiplier)
    {
        this.multiplier = multiplier;

        OnMultiplierChanged?.Invoke(multiplier);
    }

    private void SetReps(int reps)
    {
        this.reps = reps;

        OnRepsChanged?.Invoke(reps);
    }
}
