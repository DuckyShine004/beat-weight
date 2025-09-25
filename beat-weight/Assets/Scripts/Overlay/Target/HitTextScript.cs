using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class HitTextScript : MonoBehaviour
{
    public TextMeshProUGUI text;
    public CanvasGroup canvasGroup;

    private Coroutine hideRoutine;

    public void ShowText(string message)
    {
        text.text = message;

        if (message == "Perfect")
            text.color = Color.green;
        else if (message == "Early")
            text.color = Color.red;
        else
            text.color = Color.white;

        canvasGroup.alpha = 1f;

        if (hideRoutine != null)
            StopCoroutine(hideRoutine);

        hideRoutine = StartCoroutine(HideAfterDelay(1f));
    }

    private IEnumerator HideAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canvasGroup.alpha = 0f;
    }
}
