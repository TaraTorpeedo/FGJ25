using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spline : MonoBehaviour
{
    [Header("Spline Points")]
    public List<Transform> controlPoints; // Points that define the spline

    [Header("Visualization")]
    public int curveResolution = 20; // Number of points for curve smoothness
    public LineRenderer lineRenderer; // Optional: To visualize the spline

    private void OnDrawGizmos()
    {
        if (controlPoints == null || controlPoints.Count < 2)
            return;

        // Draw the spline in the Scene View
        Gizmos.color = Color.green;
        Vector3 previousPoint = controlPoints[0].position;

        for (int i = 1; i <= curveResolution; i++)
        {
            float t = i / (float)curveResolution;
            Vector3 point = GetSplinePoint(t);
            Gizmos.DrawLine(previousPoint, point);
            previousPoint = point;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        // Optional: Render the spline in-game using a LineRenderer
        if (lineRenderer != null)
        {
            UpdateLineRenderer();
        }
    }

    public void UpdateLineRenderer()
    {
        if (lineRenderer == null || controlPoints.Count < 2)
            return;

        lineRenderer.positionCount = curveResolution + 1;
        for (int i = 0; i <= curveResolution; i++)
        {
            float t = i / (float)curveResolution;
            lineRenderer.SetPosition(i, GetSplinePoint(t));
        }
    }

    public Vector3 GetSplinePoint(float t)
    {
        if (controlPoints.Count < 2)
            return Vector3.zero;

        // Use a simple linear or cubic interpolation (Bezier)
        if (controlPoints.Count == 2)
        {
            // Linear interpolation
            return Vector3.Lerp(controlPoints[0].position, controlPoints[1].position, t);
        }
        else if (controlPoints.Count == 4)
        {
            // Cubic Bezier curve
            Vector3 p0 = controlPoints[0].position;
            Vector3 p1 = controlPoints[1].position;
            Vector3 p2 = controlPoints[2].position;
            Vector3 p3 = controlPoints[3].position;

            return Mathf.Pow(1 - t, 3) * p0 +
                   3 * Mathf.Pow(1 - t, 2) * t * p1 +
                   3 * (1 - t) * Mathf.Pow(t, 2) * p2 +
                   Mathf.Pow(t, 3) * p3;
        }

        // For more than 4 control points, you can implement a Catmull-Rom or other curve
        return Vector3.zero;
    }
}
