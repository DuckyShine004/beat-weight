using System;
using UnityEngine;
using UnityEngine.UI;

public class UIOverlapHighlighter : MonoBehaviour
{
    [Header("Assign these")]
    public RectTransform circleRT;       // the small moving circle
    public RectTransform hitZoneRT;      // the small contact zone under/near the bicep
    public Image bicepImage;
    public Boolean correctForm = false;

    [Header("Colors (0..1 alpha!)")]
    public Color activeColor   = new Color(1, 1, 1, 1);   // visible
    public Color inactiveColor = new Color(1, 1, 1, 0);   // transparent

    [Tooltip("Require at least this fraction of the CIRCLE’s area to overlap the hit zone")]
    [Range(0f, 1f)] public float requireCircleCoverage = 0.20f; // 20% overlap feels “touchy”
    public float paddingPixels = 0f; // + to shrink rects, - to grow

    Canvas _canvas;

    void Awake()
    {
        _canvas = circleRT.GetComponentInParent<Canvas>();
        if (!bicepImage) bicepImage = hitZoneRT.GetComponentInParent<Image>();
    }

    void Update()
    {
        var cam = (_canvas && _canvas.renderMode == RenderMode.ScreenSpaceCamera) ? _canvas.worldCamera : null;

        Rect circle = WorldAABB(circleRT, cam, paddingPixels);
        Rect zone   = WorldAABB(hitZoneRT, cam, paddingPixels);

        Rect inter = Intersect(circle, zone);
        float coverage = (circle.width <= 0 || circle.height <= 0)
            ? 0f
            : (inter.width * inter.height) / (circle.width * circle.height);

        bool touching = coverage >= requireCircleCoverage;
        correctForm = touching;
        bicepImage.color = touching ? activeColor : inactiveColor;
    }

    static Rect WorldAABB(RectTransform rt, Camera cam, float pad)
    {
        Vector3[] w = new Vector3[4];
        rt.GetWorldCorners(w);
        for (int i = 0; i < 4; i++)
            w[i] = RectTransformUtility.WorldToScreenPoint(cam, w[i]);

        float xMin = Mathf.Min(Mathf.Min(w[0].x, w[1].x), Mathf.Min(w[2].x, w[3].x)) + pad;
        float xMax = Mathf.Max(Mathf.Max(w[0].x, w[1].x), Mathf.Max(w[2].x, w[3].x)) - pad;
        float yMin = Mathf.Min(Mathf.Min(w[0].y, w[1].y), Mathf.Min(w[2].y, w[3].y)) + pad;
        float yMax = Mathf.Max(Mathf.Max(w[0].y, w[1].y), Mathf.Max(w[2].y, w[3].y)) - pad;

        if (xMax <= xMin || yMax <= yMin) return new Rect(0,0,0,0);
        return Rect.MinMaxRect(xMin, yMin, xMax, yMax);
    }

    static Rect Intersect(Rect a, Rect b)
    {
        float xMin = Mathf.Max(a.xMin, b.xMin);
        float xMax = Mathf.Min(a.xMax, b.xMax);
        float yMin = Mathf.Max(a.yMin, b.yMin);
        float yMax = Mathf.Min(a.yMax, b.yMax);
        if (xMax <= xMin || yMax <= yMin) return new Rect(0,0,0,0);
        return Rect.MinMaxRect(xMin, yMin, xMax, yMax);
    }
}