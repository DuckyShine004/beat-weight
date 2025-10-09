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

    void Start()
    {
        timer = timerDuration;
    }

    void CleanupScene()
    {
        //Find all game objects of type EnemyAI
        EnemyAI[] enemies = FindObjectsByType<EnemyAI>(FindObjectsSortMode.None);

        foreach (EnemyAI enemy in enemies)
        {
            enemy.OnDeath();
        }
    }

    void OnGameEnd()
    {
        endScreen.SetActive(true);

        DisableGUI();

        // Perform scene cleanup
        CleanupScene();

        // Finally disable the game
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

    // Update is called once per frame
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
