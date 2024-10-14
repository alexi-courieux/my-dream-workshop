using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
[AddComponentMenu("UI/Arrow")]
public class UIArrowSpline : Graphic
{
    public List<Vector2> points;
    
    float width;
    float height;
    
    public float thickness = 10f;
    public float arrowSize = 20f;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        if (points.Count < 2)
        {
            return;
        }

        List<Vector3> splinePoints = new List<Vector3>();

        for (int i = 0; i < points.Count - 1; i++)
        {
            Vector3 p0 = i == 0 ? points[i] : points[i - 1];
            Vector3 p1 = points[i];
            Vector3 p2 = points[i + 1];
            Vector3 p3 = i == points.Count - 2 ? points[i + 1] : points[i + 2];

            for (float t = 0; t < 1; t += 0.1f)
            {
                splinePoints.Add(CatmullRom(p0, p1, p2, p3, t));
            }
        }

        for (int i = 0; i < splinePoints.Count - 1; i++)
        {
            Vector3 point = splinePoints[i];
            Vector3 nextPoint = splinePoints[i + 1];
            Vector3 direction = (nextPoint - point).normalized;

            DrawVerticesForPoint(point, direction, vh);
        }

        int vertexCount = vh.currentVertCount;
        for (int i = 0; i < splinePoints.Count - 1; i++)
        {
            int index = i * 2;
            if (index + 3 < vertexCount)
            {
                vh.AddTriangle(index + 0, index + 1, index + 3);
                vh.AddTriangle(index + 3, index + 2, index + 0);
            }
        }

        // Draw arrow tip
        Vector3 lastPoint = splinePoints[splinePoints.Count - 1];
        Vector3 secondLastPoint = splinePoints[splinePoints.Count - 2];
        Vector3 lastDirection = (lastPoint - secondLastPoint).normalized;

        DrawArrowTip(lastPoint, lastDirection, vh);
    }

    private void DrawArrowTip(Vector3 point, Vector3 direction, VertexHelper vh)
    {
        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;

        Vector3 perpendicular = Vector3.Cross(direction, Vector3.forward).normalized * (arrowSize / 2);
        Vector3 tipPoint1 = point + direction * (arrowSize * 0.5f); // Reduce the offset here
        Vector3 tipPoint2 = point - perpendicular;
        Vector3 tipPoint3 = point + perpendicular;

        vertex.position = point;
        vh.AddVert(vertex);

        vertex.position = tipPoint1;
        vh.AddVert(vertex);

        vertex.position = tipPoint2;
        vh.AddVert(vertex);

        vertex.position = tipPoint3;
        vh.AddVert(vertex);

        int offset = vh.currentVertCount - 4;
        vh.AddTriangle(offset, offset + 1, offset + 2);
        vh.AddTriangle(offset, offset + 3, offset + 1);
    }
    
    private void DrawVerticesForPoint(Vector3 point, Vector3 direction, VertexHelper vh)
    {
        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;

        Vector3 perpendicular = Vector3.Cross(direction, Vector3.forward).normalized * (thickness / 2);

        vertex.position = point - perpendicular;
        vh.AddVert(vertex);

        vertex.position = point + perpendicular;
        vh.AddVert(vertex);
    }
    
    private Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float t2 = t * t;
        float t3 = t2 * t;

        return 0.5f * (
            (2.0f * p1) +
            (-p0 + p2) * t +
            (2.0f * p0 - 5.0f * p1 + 4.0f * p2 - p3) * t2 +
            (-p0 + 3.0f * p1 - 3.0f * p2 + p3) * t3
        );
    }
}