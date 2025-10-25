using UnityEngine;

/// <summary>
/// Controls the in-game tutorial sequence, cycling through tutorial steps (panels or prompts)
/// and enabling affected gameplay objects once the tutorial ends.
/// </summary>
public class Tutorial : MonoBehaviour
{
    [Header("Tutorial attributes")]
    public GameObject[] tutorials;

    [Header("References")]
    [SerializeField]
    private GameObject[] affectedObjects;

    private int tutorialIndex;

    /// <summary>
    /// Initialises the tutorial when the game starts by showing the first tutorial step.
    /// </summary>
    private void Start()
    {
        tutorialIndex = 0;

        UpdateTutorial();
    }

    /// <summary>
    /// Resets and restarts the tutorial sequence whenever the object is re-enabled.
    /// </summary>
    private void OnEnable()
    {
        ResetTutorial();

        UpdateTutorial();
    }

    /// <summary>
    /// Advances to the next tutorial step.
    /// Called typically by a UI button or interaction event.
    /// </summary>
    public void NextTutorial()
    {
        ++tutorialIndex;

        UpdateTutorial();
    }

    /// <summary>
    /// Updates the tutorial UI based on the current step index.
    /// Handles progression, deactivation of previous steps, and completion logic.
    /// </summary>
    private void UpdateTutorial()
    {
        if (tutorials.Length == 0)
        {
            return;
        }

        if (tutorialIndex > 0)
        {
            tutorials[tutorialIndex - 1].SetActive(false);
        }

        if (tutorialIndex < tutorials.Length)
        {
            tutorials[tutorialIndex].SetActive(true);
        }
        else
        {
            ResetTutorial();

            OnTutorialClose();
        }
    }

    /// <summary>
    /// Resets all tutorial steps to inactive and restarts the index.
    /// </summary>
    private void ResetTutorial()
    {
        foreach (var tutorial in tutorials)
        {
            tutorial.SetActive(false);
        }

        tutorialIndex = 0;
    }

    /// <summary>
    /// Called when the tutorial sequence finishes.
    /// Re-enables all affected gameplay objects and disables the tutorial system.
    /// </summary>
    private void OnTutorialClose()
    {
        foreach (var gObject in affectedObjects)
        {
            gObject.SetActive(true);
        }

        gameObject.SetActive(false);
    }
}
