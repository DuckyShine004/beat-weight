using UnityEngine;

[ExecuteAlways]
public class HandManager : MonoBehaviour
{
    public enum Hand { Left, Right }
    [Header("Which hand is active?")]
    public Hand activeHand = Hand.Left;

    [Header("GameObjects to toggle")]
    public GameObject[] leftGameObjects;
    public GameObject[] rightGameObjects;

    [Header("Scripts (Components) to toggle")]
    public Behaviour[] leftScripts;
    public Behaviour[] rightScripts;

    void OnEnable() => Apply();
#if UNITY_EDITOR
    void OnValidate() { if (isActiveAndEnabled) Apply(); }
#endif

    public void Apply()
    {
        bool leftOn = (activeHand == Hand.Left);

        // Toggle GameObjects
        ToggleGameObjects(leftGameObjects, leftOn);
        ToggleGameObjects(rightGameObjects, !leftOn);

        // Toggle Scripts
        ToggleScripts(leftScripts, leftOn);
        ToggleScripts(rightScripts, !leftOn);
    }

    void ToggleGameObjects(GameObject[] objs, bool enable)
    {
        foreach (var go in objs)
        {
            if (go) go.SetActive(enable);
        }
    }

    void ToggleScripts(Behaviour[] scripts, bool enable)
    {
        foreach (var s in scripts)
        {
            if (s) s.enabled = enable;
        }
    }
}
