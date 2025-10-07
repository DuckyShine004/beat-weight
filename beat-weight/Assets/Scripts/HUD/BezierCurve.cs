using UnityEngine;

[ExecuteAlways]
public class BezierCurve : MonoBehaviour
{
    public Transform p0, p1, p2, p3; // Control points

    // Returns a position along the curve given t ∈ [0, 1]
    public Vector3 GetPoint(float t)
    {
        float u = 1 - t;
        return u * u * u * p0.position +
               3 * u * u * t * p1.position +
               3 * u * t * t * p2.position +
               t * t * t * p3.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Vector3 prev = p0.position;
        for (int i = 1; i <= 30; i++)
        {
            float t = i / 30f;
            Vector3 point = GetPoint(t);
            Gizmos.DrawLine(prev, point);
            prev = point;
        }
    }
}