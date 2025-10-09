using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [Header("Tutorial attributes")]
    public GameObject[] tutorials;

    [Header("References")]
    [SerializeField]
    private GameObject[] affectedObjects;

    private int tutorialIndex;

    void Start()
    {
        tutorialIndex = 0;

        UpdateTutorial();
    }

    void OnEnable()
    {
        ResetTutorial();

        UpdateTutorial();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ++tutorialIndex;

            UpdateTutorial();
        }
    }

    void UpdateTutorial()
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

    void ResetTutorial()
    {
        foreach (var tutorial in tutorials)
        {
            tutorial.SetActive(false);
        }

        tutorialIndex = 0;
    }

    // CALL LAST
    void OnTutorialClose()
    {
        foreach (var gObject in affectedObjects)
        {
            gObject.SetActive(true);
        }

        gameObject.SetActive(false);
    }
}
