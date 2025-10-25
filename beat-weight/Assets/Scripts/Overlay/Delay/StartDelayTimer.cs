using System;
using TMPro;
using UnityEngine;

/// <summary>
/// Handles a short countdown timer before the game officially starts.
/// Displays the remaining delay time, updates the UI each frame,
/// and hides specified objects once the countdown reaches zero.
/// </summary>
public class StartDelayTimer : MonoBehaviour
{
    [SerializeField]
    private float delay;

    [SerializeField]
    private TMP_Text timerText;

    [Header("Hide Objects")]
    public GameObject[] objectsToHide;

    /// <summary>
    /// Sets the delay duration dynamically (useful for programmatically controlling start times).
    /// </summary>
    /// <param name="delay">The delay duration in seconds.</param>
    public void SetDelay(float delay)
    {
        this.delay = delay;
    }

    /// <summary>
    /// Called once the countdown reaches zero.
    /// Hides all specified objects to start the game.
    /// </summary>
    private void StartGame()
    {
        foreach (var objectToHide in objectsToHide)
        {
            objectToHide.SetActive(false);
        }
    }

    /// <summary>
    /// Updates the countdown timer each frame and displays the remaining time with 2 decimal precision.
    /// </summary>
    private void UpdateTimer()
    {
        delay -= Time.deltaTime;

        float roundedDelay = (float)Math.Round(delay, 2);

        timerText.text = $"{roundedDelay} s";
    }

    /// <summary>
    /// Unity’s Update loop- counts down the timer until zero,
    /// updates the displayed text, and starts the game when time expires.
    /// </summary>
    private void Update()
    {
        if (delay > 0.0f)
        {
            UpdateTimer();
        }
        else
        {
            delay = 0.0f;

            StartGame();
        }
    }
}
