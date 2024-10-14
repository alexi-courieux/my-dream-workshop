using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
[AddComponentMenu("UI/LineRenderer")]
public class UIGridLineRenderer : Graphic
{
    public UIGridRenderer gridRenderer;
    public Vector2Int gridSize = new Vector2Int(1, 1);
    
    public List<Vector2> points;
    
    float width;
    float height;
    float unitWidth;
    float unitHeight;
    
    public float thickness = 10f;

    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();

        width = rectTransform.rect.width;
        height = rectTransform.rect.height;

        unitWidth = width / gridSize.x;
        unitHeight = height / gridSize.y;

        if (points.Count < 2)
        {
            return;
        }

        for (int i = 0; i < points.Count - 1; i++)
        {
            Vector2 point = points[i];
            Vector2 nextPoint = points[i + 1];
            Vector2 direction = (nextPoint - point).normalized;

            DrawVerticesForPoint(point, direction, vh);
            DrawVerticesForPoint(nextPoint, direction, vh);
        }

        for (int i = 0; i < points.Count - 1; i++)
        {
            int index = i * 4;
            vh.AddTriangle(index + 0, index + 1, index + 2);
            vh.AddTriangle(index + 2, index + 1, index + 3);
        }
    }

    private void DrawVerticesForPoint(Vector2 point, Vector2 direction, VertexHelper vh)
    {
        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;

        Vector3 perpendicular = Vector3.Cross(direction, Vector3.forward).normalized * (thickness / 2);

        vertex.position = new Vector3(point.x, point.y) - perpendicular;
        vertex.position += new Vector3(unitWidth * point.x, unitHeight * point.y);
        vh.AddVert(vertex);

        vertex.position = new Vector3(point.x, point.y) + perpendicular;
        vertex.position += new Vector3(unitWidth * point.x, unitHeight * point.y);
        vh.AddVert(vertex);
    }

    private void Update()
    {
        if (gridRenderer is null) return;
        if (gridSize == gridRenderer.gridSize) return;
        
        gridSize = gridRenderer.gridSize;
        SetVerticesDirty();
    }
}