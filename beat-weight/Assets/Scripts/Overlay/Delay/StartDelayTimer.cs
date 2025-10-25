using System;
using TMPro;
using UnityEngine;

public class StartDelayTimer : MonoBehaviour
{
    [SerializeField]
    private float delay;

    [SerializeField]
    private TMP_Text timerText;

    [Header("Hide Objects")]
    public GameObject[] objectsToHide;

    public void SetDelay(float delay)
    {
        this.delay = delay;
    }

    private void StartGame()
    {
        foreach (var objectToHide in objectsToHide)
        {
            objectToHide.SetActive(false);
        }
    }

    private void UpdateTimer()
    {
        delay -= Time.deltaTime;

        float roundedDelay = (float)Math.Round(delay, 2);

        timerText.text = $"{roundedDelay} s";
    }

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
