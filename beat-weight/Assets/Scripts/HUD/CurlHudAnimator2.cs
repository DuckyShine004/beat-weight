using UnityEngine;

public class CurlHUDAnimator2 : MonoBehaviour
{
    public UnityEngine.UI.Image bicepImage;

    public ControllerHeightsToDots controllerHeightsToDots;
    public HandManager handManager;
    private RectTransform selectedDot;

    void Start()
    {
        if (handManager.activeHand == HandManager.Hand.Left)
        {
            selectedDot = controllerHeightsToDots.leftDot;
        }
        else
        {
            selectedDot = controllerHeightsToDots.rightDot;
        }
    }

    void Update()
    {
        // Upper level = 50 and lower = -50
        // Debug.Log(selectedDot?.anchoredPosition.y);
        if (selectedDot)
        {
            float y = selectedDot.anchoredPosition.y;
            float normalized = Mathf.InverseLerp(-50f, 50f, y);
            // 40 bicep image frames 1-40
            int frame = Mathf.Clamp(Mathf.RoundToInt(20f - (normalized * 20f)), 1, 20);
            bicepImage.sprite = Resources.Load<Sprite>($"Textures/BicepFrames/{frame}");
        }
    }
}
