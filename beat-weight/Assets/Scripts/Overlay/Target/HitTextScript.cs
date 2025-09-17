using System;
using TMPro;
using UnityEngine;

public class HitTextScript : MonoBehaviour
{
    public TextMeshProUGUI text;
    public CanvasGroup canvasGroup;

    void Start()
    {

    }

    void Update()
    {

    }

    public void ShowText(String message)
    {
        text.text = message;
        if (message == "Perfect")
        {
            text.color = Color.green;
        }
        else if (message == "Early")
        {
            text.color = Color.red;
        }
    }
}
