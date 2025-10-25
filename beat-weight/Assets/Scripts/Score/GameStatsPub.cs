using System;
using UnityEngine;

/// <summary>
/// Publishes and manages game statistics such as score, multiplier,
/// repetitions (reps), and enemies killed.
/// Provides event-driven updates for UI and other systems to subscribe to.
/// </summary>
public class GameStatsPub : MonoBehaviour
{
    [Header("Base game stats")]
    public float baseScore;
    public float baseBeatSyncScore;
    public float baseMultiplier;

    /// <summary>
    /// The player's current score.
    /// </summary>
    public float score { get; private set; }

    /// <summary>
    /// The current multiplier value applied to the score.
    /// </summary>
    public float multiplier { get; private set; }

    /// <summary>
    /// The total number of successful reps (actions) performed.
    /// </summary>
    public int reps { get; private set; }

    /// <summary>
    /// The total number of enemies defeated.
    /// </summary>
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

    /// <summary>
    /// Initialises the game stats at the start of the game.
    /// </summary>
    private void Start()
    {
        Reset();
    }

    /// <summary>
    /// Called when an enemy is killed.
    /// Increments the kill count and rewards a successful rep.
    /// </summary>
    public void OnEnemyKilled()
    {
        IncrementEnemiesKilled();

        OnSuccessfulRep();
    }

    /// <summary>
    /// Called when the player performs a successful rep (e.g., hitting an enemy correctly).
    /// Increases score, multiplier, and total reps.
    /// </summary>
    public void OnSuccessfulRep()
    {
        AddScore();

        IncrementMultiplier();
        IncrementReps();
    }

    /// <summary>
    /// Resets all game stats to their initial values.
    /// </summary>
    public void Reset()
    {
        SetScore(INITIAL_SCORE);
        SetMultiplier(INITIAL_MULTIPLIER);
        SetReps(INITIAL_REPS);
        SetEnemiesKilled(INITIAL_ENEMIES_KILLED);
    }

    /// <summary>
    /// Called when a rep fails (e.g., player misses or mistimes action).
    /// Resets multiplier to 1 and triggers <see cref="OnFailedRepEvent"/>.
    /// </summary>
    public void OnFailedRep()
    {
        SetMultiplier(1.0f);

        OnFailedRepEvent?.Invoke();
    }

    /// <summary>
    /// Called when an action is performed in sync with the beat.
    /// Grants an additional beat-sync score bonus.
    /// </summary>
    public void OnBeatSync()
    {
        AddBeatSyncScore();
    }

    /// <summary>
    /// Adds to the score based on base score and current multiplier.
    /// </summary>
    public void AddScore()
    {
        score += baseScore * multiplier;

        SetScore(score);
    }

    /// <summary>
    /// Adds a beat-synchronised score bonus.
    /// </summary>
    private void AddBeatSyncScore()
    {
        score += baseBeatSyncScore;

        SetScore(score);
    }

    /// <summary>
    /// Increments the multiplier by the base multiplier amount.
    /// </summary>
    private void IncrementMultiplier()
    {
        multiplier += baseMultiplier;

        SetMultiplier(multiplier);
    }

    /// <summary>
    /// Increments the total number of successful reps.
    /// </summary>
    private void IncrementReps()
    {
        ++reps;

        SetReps(reps);
    }

    /// <summary>
    /// Increments the total number of enemies killed.
    /// </summary>
    private void IncrementEnemiesKilled()
    {
        ++enemiesKilled;

        SetEnemiesKilled(enemiesKilled);
    }

    /// <summary>
    /// Sets the score and triggers <see cref="OnScoreChanged"/>.
    /// </summary>
    private void SetScore(float score)
    {
        this.score = score;

        OnScoreChanged?.Invoke(score);
    }

    /// <summary>
    /// Sets the multiplier and triggers <see cref="OnMultiplierChanged"/>.
    /// </summary>
    private void SetMultiplier(float multiplier)
    {
        this.multiplier = multiplier;

        OnMultiplierChanged?.Invoke(multiplier);
    }

    /// <summary>
    /// Sets the rep count and triggers <see cref="OnRepsChanged"/>.
    /// </summary>
    private void SetReps(int reps)
    {
        this.reps = reps;

        OnRepsChanged?.Invoke(reps);
    }

    /// <summary>
    /// Sets the enemies killed count and triggers <see cref="OnEnemiesKilled"/>.
    /// </summary>
    private void SetEnemiesKilled(int enemiesKilled)
    {
        this.enemiesKilled = enemiesKilled;

        OnEnemiesKilled?.Invoke(enemiesKilled);
    }
}
