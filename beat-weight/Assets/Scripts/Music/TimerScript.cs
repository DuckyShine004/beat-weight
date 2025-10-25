using TMPro;
using UnityEngine;

/// <summary>
/// Manages the countdown timer for the game session.
/// Displays remaining time, handles end-of-game events,
/// cleans up active objects, and plays game-over effects.
/// </summary>
public class TimerScript : MonoBehaviour
{
    [Header("Timer Attributes")]
    public float timerDuration;
    public float timer;
    public GameObject endScreen;
    public TextMeshProUGUI timerText;

    [Header("GUI Objects")]
    public GameObject[] guiObjects;

    [Header("Cleanup Objects")]
    public GameObject[] cleanupObjects;

    [Header("Audio Manager")]
    public AudioManager audioManager;

    [Header("Game Over Effects")]
    public GameObject gameOverEffect;
    public AudioClip gameOverSoundEffect;

    /// <summary>
    /// Initialises the timer at the start of the game.
    /// </summary>
    private void Start()
    {
        timer = timerDuration;
    }

    /// <summary>
    /// Resets the timer when the script becomes enabled.
    /// </summary>
    private void OnEnable()
    {
        timer = timerDuration;
    }

    /// <summary>
    /// Cleans up all active enemies in the scene by triggering their death logic.
    /// </summary>
    private void CleanupScene()
    {
        EnemyAI[] enemies = FindObjectsByType<EnemyAI>(FindObjectsSortMode.None);

        foreach (EnemyAI enemy in enemies)
        {
            enemy.OnDeath(false);
        }
    }

    /// <summary>
    /// Spawns the game-over visual effects and plays the corresponding sound.
    /// </summary>
    private void PlayGameOverEffects()
    {
        Instantiate(gameOverEffect);

        audioManager.PlaySoundEffect(gameOverSoundEffect);
    }

    /// <summary>
    /// Handles all logic when the game timer reaches zero.
    /// Displays the end screen, stops gameplay, and triggers cleanup.
    /// </summary>
    private void OnGameEnd()
    {
        endScreen.SetActive(true);

        PlayGameOverEffects();

        DisableGUI();
        CleanupScene();
        DisableGame();
    }

    /// <summary>
    /// Disables all gameplay-related objects listed in <see cref="cleanupObjects"/>.
    /// </summary>
    private void DisableGame()
    {
        foreach (var cleanupObject in cleanupObjects)
        {
            cleanupObject.SetActive(false);
        }
    }

    /// <summary>
    /// Disables all GUI elements listed in <see cref="guiObjects"/>.
    /// </summary>
    private void DisableGUI()
    {
        foreach (var guiObject in guiObjects)
        {
            guiObject.SetActive(false);
        }
    }

    /// <summary>
    /// Updates the countdown timer each frame.
    /// When time runs out, the game ends.
    /// </summary>
    private void Update()
    {
        if (timer > 0f)
        {
            timer -= Time.deltaTime;

            if (timerText != null)
            {
                timerText.text = "Time left: " + Mathf.Ceil(timer).ToString();
            }

            if (timer <= 0f)
            {
                OnGameEnd();
            }
        }
    }
}
