using UnityEngine;

public class Tutorial : MonoBehaviour
{
    [Header("Tutorial attributes")]
    public GameObject[] tutorials;

    [Header("References")]
    [SerializeField]
    private GameObject startMenu;

    private int tutorialIndex;

    void Start()
    {
        tutorialIndex = 0;

        UpdateTutorial();
    }

    void OnEnable()
    {
        ResetTutorialState();

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
            ResetTutorialState();

            OnTutorialClose();
        }
    }

    void ResetTutorialState()
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
        startMenu.SetActive(true);

        gameObject.SetActive(false);
    }
}
