using System;
using UnityEngine;

public class GameStatsPub : MonoBehaviour
{
    [Header("Base game stats")]
    public float baseScore;
    public float baseBeatSyncScore;
    public float baseMultiplier;

    public float score { get; private set; }
    public float multiplier { get; private set; }
    public int reps { get; private set; }
    public int enemiesKilled { get; private set; }

    private const float INITIAL_SCORE = 0.0f;
    private const float INITIAL_MULTIPLIER = 1.0f;
    private const int INITIAL_REPS = 0;
    private const int INITIAL_ENEMIES_KILLED = 0;

    public event Action<float> OnScoreChanged;
    public event Action<float> OnMultiplierChanged;
    public event Action<int> OnRepsChanged;
    public event Action<int> OnEnemiesKilled;
    public event Action OnFailedRepEvent;

    public void Start()
    {
        Reset();
    }

    public void OnEnemyKilled()
    {
        IncrementEnemiesKilled();

        OnSuccessfulRep();
    }

    public void OnSuccessfulRep()
    {
        AddScore();

        IncrementMultiplier();
        IncrementReps();
    }

    public void Reset()
    {
        SetScore(INITIAL_SCORE);
        SetMultiplier(INITIAL_MULTIPLIER);
        SetReps(INITIAL_REPS);
        SetEnemiesKilled(INITIAL_ENEMIES_KILLED);
    }

    public void OnFailedRep()
    {
        SetMultiplier(1.0f);

        OnFailedRepEvent?.Invoke();
    }

    public void OnBeatSync()
    {
        AddBeatSyncScore();
    }

    public void AddScore()
    {
        score += baseScore * multiplier;

        SetScore(score);
    }

    private void AddBeatSyncScore()
    {
        score += baseBeatSyncScore;

        SetScore(score);
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

    private void IncrementEnemiesKilled()
    {
        ++enemiesKilled;

        SetEnemiesKilled(enemiesKilled);
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

    private void SetEnemiesKilled(int enemiesKilled)
    {
        this.enemiesKilled = enemiesKilled;

        OnEnemiesKilled?.Invoke(enemiesKilled);
    }
}
