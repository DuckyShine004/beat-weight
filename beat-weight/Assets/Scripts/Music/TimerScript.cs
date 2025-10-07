using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class TimerScript : MonoBehaviour
{
    public float timerDuration;
    public float timer;
    public GameObject endScreen;
    public TextMeshProUGUI timerText;
    void Start()
    {
        timer = timerDuration;

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
                endScreen.SetActive(true);
            }
        }
    }
}
