using System;
using System.Collections;
using TMPro;
using UnityEngine;

/// <summary>
/// Subscribes to <see cref="GameStatsPub"/> events to update the on-screen UI in real time.
/// Displays the player's score, multiplier, and reps.
/// Also provides a visual flash effect when a rep fails.
/// </summary>
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

    /// <summary>
    /// Caches the original text colour when the component is first initialised.
    /// </summary>
    private void Awake()
    {
        originalColour = scoreText.color;
    }

    /// <summary>
    /// Subscribes to all <see cref="GameStatsPub"/> events and initialises the UI with current values.
    /// </summary>
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

    /// <summary>
    /// Updates the displayed score text with a rounded value.
    /// </summary>
    /// <param name="score">The latest score value from <see cref="GameStatsPub"/>.</param>
    private void UpdateScore(float score)
    {
        float roundedScore = (float)Math.Round(score, 2);

        scoreText.text = $"${roundedScore}";
    }

    /// <summary>
    /// Updates the displayed multiplier text with a rounded value.
    /// </summary>
    /// <param name="multiplier">The latest multiplier value from <see cref="GameStatsPub"/>.</param>
    private void UpdateMultiplier(float multiplier)
    {
        float roundedMultipler = (float)Math.Round(multiplier, 1);

        multiplierText.text = $"{roundedMultipler} x";
    }

    /// <summary>
    /// Updates the displayed rep count.
    /// </summary>
    /// <param name="reps">The latest rep count from <see cref="GameStatsPub"/>.</param>
    private void UpdateReps(int reps)
    {
        repText.text = $"{reps}";
    }

    /// <summary>
    /// Triggers the fail-colour flash effect for all displayed text elements.
    /// </summary>
    private void FlashAllFailColours()
    {
        StartCoroutine(FlashFailColour(scoreText));
        StartCoroutine(FlashFailColour(multiplierText));
        StartCoroutine(FlashFailColour(repsText));
        StartCoroutine(FlashFailColour(repText));
    }

    /// <summary>
    /// Coroutine that flashes a given text element to the fail colour and restores its original colour after a delay.
    /// </summary>
    /// <param name="text">The text element to flash.</param>
    private IEnumerator FlashFailColour(TMP_Text text)
    {
        text.color = failColour;

        yield return new WaitForSeconds(failDuration);

        text.color = originalColour;
    }
}
