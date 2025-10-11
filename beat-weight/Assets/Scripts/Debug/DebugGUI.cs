using UnityEngine;

public class DebugGUI : MonoBehaviour
{
    [Header("Debug GUI Dimensions")]
    public float width;
    public float height;
    public float x;
    public float y;

    [Header("Debug Information")]
    public DebugEntry[] debugEntries;

    private float INITIAL_X = 0.0f;
    private float INITIAL_Y = 0.0f;

    private void Start()
    {
        x = INITIAL_X;
        y = INITIAL_Y;
    }

    private void OnGUI()
    {
        Matrix4x4 guiMatrix = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, Vector3.one);

        Rect GUIRect = new Rect(x, y, width, height);

        GUILayout.BeginArea(GUIRect, GUI.skin.box);

        DebugInformation();

        GUILayout.EndArea();
    }

    private void Debug(string name, Object obj)
    {
        GUILayout.Label($"{name}: {obj.ToString()}");
    }

    private void DebugInformation()
    {
        foreach (DebugEntry debugEntry in debugEntries)
        {
            GUILayout.Label($"{debugEntry.key}: {debugEntry.value}");
        }
    }
}
