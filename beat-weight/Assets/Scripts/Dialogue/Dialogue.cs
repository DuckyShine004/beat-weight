using System.Collections;
using TMPro;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    [Header("Dialogue attributes")]
    public TextMeshProUGUI textComponent;
    public string[] lines;
    public float textSpeed;

    [Header("Sound effect attributes")]
    public AudioSource audioSource;
    public AudioClip talkingClip;

    private int characterIndex;

    void Start()
    {
        textComponent.text = string.Empty;

        StartDialogue();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == lines[characterIndex])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();

                textComponent.text = lines[characterIndex];
            }
        }
    }

    private void StartDialogue()
    {
        characterIndex = 0;

        StartCoroutine(TypeLine());
    }

    private IEnumerator TypeLine()
    {
        foreach (char character in lines[characterIndex].ToCharArray())
        {
            textComponent.text += character;

            audioSource.PlayOneShot(talkingClip);

            yield return new WaitForSeconds(textSpeed);
        }
    }

    private void NextLine()
    {
        if (characterIndex < lines.Length - 1)
        {
            ++characterIndex;

            textComponent.text = string.Empty;

            StartCoroutine(TypeLine());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
