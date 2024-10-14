using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
[AddComponentMenu("UI/GridRenderer")]
public class UIGridRenderer : Graphic
{
    public Vector2Int gridSize = new Vector2Int(1, 1);
    public float thickness = 10f;
    
    float width;
    float height;
    float cellWidth;
    float cellHeight;
    
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        
        width = rectTransform.rect.width;
        height = rectTransform.rect.height;
        
        cellWidth = width / gridSize.x;
        cellHeight = height / gridSize.y;
        
        int count = 0;
        
        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                DrawCell(x, y, count++, vh);
            }
        }
    }
    private void DrawCell(int x, int y, int index, VertexHelper vh)
    {
        float xPos = x * cellWidth;
        float yPos = y * cellHeight;

        UIVertex vertex = UIVertex.simpleVert;
        vertex.color = color;

        vertex.position = new Vector3(xPos, yPos);
        vh.AddVert(vertex); // 0 - bottom left
        
        vertex.position = new Vector3(xPos, yPos + cellHeight);
        vh.AddVert(vertex); // 1 - top left
        
        vertex.position = new Vector3(xPos + cellWidth, yPos + cellHeight);
        vh.AddVert(vertex); // 2 - top right
        
        vertex.position = new Vector3(xPos + cellWidth, yPos);
        vh.AddVert(vertex); // 3 - bottom right
        
        float widthSqr = thickness * thickness;
        float distanceSqr = widthSqr / 2;
        float distance = Mathf.Sqrt(distanceSqr);

        vertex.position = new Vector3(xPos + distance, yPos + distance);
        vh.AddVert(vertex); // 4 - bottom left
        
        vertex.position = new Vector3(xPos + distance, yPos + cellHeight - distance);
        vh.AddVert(vertex); // 5 - top left

        vertex.position = new Vector3(xPos + cellWidth - distance, yPos + cellHeight - distance);
        vh.AddVert(vertex); // 6 - top right
        
        vertex.position = new Vector3(xPos + cellWidth - distance, yPos + distance);
        vh.AddVert(vertex); // 7 - bottom right
        
        int offset = index * 8;

        // Top edge 1256
        vh.AddTriangle(offset + 1, offset + 2, offset + 5);
        vh.AddTriangle(offset + 5, offset + 6, offset + 2);
        
        // Right edge 2367
        vh.AddTriangle(offset + 2, offset + 3, offset + 6);
        vh.AddTriangle(offset + 6, offset + 7 ,offset + 3);
        
        // Bottom edge 0347
        vh.AddTriangle(offset + 3, offset + 0, offset + 7);
        vh.AddTriangle(offset + 7, offset + 4, offset + 0);
        
        // Left edge 0145
        vh.AddTriangle(offset + 0, offset + 1, offset + 4);
        vh.AddTriangle(offset + 4, offset + 5, offset + 1);
    }
}