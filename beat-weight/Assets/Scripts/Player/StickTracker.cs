using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class StickTracker : MonoBehaviour
{
    [SerializeField] private Transform leftControllerVisual;
    [SerializeField] private Transform rightControllerVisual;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
{
        if (leftControllerVisual == null)
            Debug.LogError("Left controller visual is not assigned!");

        if (rightControllerVisual == null)
            Debug.LogError("Right controller visual is not assigned!");
}

    void Update()
{
    if (leftControllerVisual)
        Debug.Log("Left controller visual height: " + leftControllerVisual.position.y.ToString("F2"));

    if (rightControllerVisual)
        Debug.Log("Right controller visual height: " + rightControllerVisual.position.y.ToString("F2"));
}
}
