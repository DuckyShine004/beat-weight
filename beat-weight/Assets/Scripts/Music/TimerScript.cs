using TMPro;
using UnityEngine;

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

    void Start()
    {
        timer = timerDuration;
    }

    void OnEnable()
    {
        timer = timerDuration;
    }

    void CleanupScene()
    {
        EnemyAI[] enemies = FindObjectsByType<EnemyAI>(FindObjectsSortMode.None);

        foreach (EnemyAI enemy in enemies)
        {
            enemy.OnDeath(false);
        }
    }

    void PlayGameOverEffects()
    {
        Instantiate(gameOverEffect);

        audioManager.PlaySoundEffect(gameOverSoundEffect);
    }

    void OnGameEnd()
    {
        endScreen.SetActive(true);

        PlayGameOverEffects();

        DisableGUI();
        CleanupScene();
        DisableGame();
    }

    void DisableGame()
    {
        foreach (var cleanupObject in cleanupObjects)
        {
            cleanupObject.SetActive(false);
        }
    }

    void DisableGUI()
    {
        foreach (var guiObject in guiObjects)
        {
            guiObject.SetActive(false);
        }
    }

    void Update()
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
