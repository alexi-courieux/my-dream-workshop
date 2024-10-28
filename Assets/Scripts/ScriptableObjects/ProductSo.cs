using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Product", menuName = "ScriptableObject/Product")]
public class ProductSo : ScriptableObject
{
    public Transform prefab;
    public Sprite sprite;
    public string itemName;
    public int buyPrice;
    public int sellPrice;
    public ProductTypeSo[] types;

    public override bool Equals(object other)
    {
        if (other is ProductSo product)
        {
            return product.itemName == itemName;
        }

        return false;
    }
    
    public override int GetHashCode()
    {
        return itemName.GetHashCode();
    }
}

[CustomEditor(typeof(ProductSo), true)]
public class ProductSoEditor : Editor
{
    private List<ProductTypeSo> availableTypes;
    private bool[] selectedTypes = Array.Empty<bool>();

    private void OnEnable()
    {
        string[] typesGuids = AssetDatabase.FindAssets($"t:{nameof(ProductTypeSo)}");
        availableTypes = typesGuids
            .Select(guid => AssetDatabase.LoadAssetAtPath<ProductTypeSo>(AssetDatabase.GUIDToAssetPath(guid)))
            .Where(type => type.assignable)
            .OrderBy(type => type.GetPath())
            .ToList();
        ProductSo productSo = (ProductSo)target;
        selectedTypes = availableTypes.Select(type => productSo.types.Contains(type)).ToArray();
    }

    public override void OnInspectorGUI()
    {
        // Get the target object
        ProductSo productSo = (ProductSo)target;

        // Draw default inspector
        DrawDefaultInspector();

        // Display dropdown list for selecting tags
        EditorGUILayout.LabelField($"Available Tags ({availableTypes.Count}):");

        HashSet<ProductTypeSo> displayedParents = new HashSet<ProductTypeSo>();
        for (int i = 0; i < availableTypes.Count; i++)
        {
            if (availableTypes[i].parentType is not null && !displayedParents.Contains(availableTypes[i].parentType))
            {
                DisplayParentHierarchy(availableTypes[i].parentType, displayedParents);
            }

            int indentLevel = GetIndentLevel(availableTypes[i]);
            string indent = new string(' ', indentLevel * 2 - 2); // Adjusted indentation
            string label = indent + availableTypes[i].typeName;
            bool newSelected = EditorGUILayout.ToggleLeft(label, selectedTypes[i]);

            if (newSelected == selectedTypes[i]) continue;

            selectedTypes[i] = newSelected;
            List<ProductTypeSo> selectedProductTypes = new List<ProductTypeSo>();
            for (int j = 0; j < selectedTypes.Length; j++)
            {
                if (selectedTypes[j])
                {
                    selectedProductTypes.Add(availableTypes[j]);
                }
            }
            productSo.types = selectedProductTypes.ToArray();

            // Apply changes and save asset
            EditorUtility.SetDirty(productSo);
            AssetDatabase.SaveAssets();
        }
    }

    private void DisplayParentHierarchy(ProductTypeSo parentType, HashSet<ProductTypeSo> displayedParents)
    {
        if (parentType is null || displayedParents.Contains(parentType)) return;

        List<ProductTypeSo> hierarchy = new List<ProductTypeSo>();
        while (parentType is not null && !displayedParents.Contains(parentType))
        {
            hierarchy.Add(parentType);
            displayedParents.Add(parentType);
            parentType = parentType.parentType;
        }

        hierarchy.Reverse();
        for (int i = 0; i < hierarchy.Count; i++)
        {
            ProductTypeSo type = hierarchy[i];
            int indentLevel = GetIndentLevel(type);
            string indent = new string(' ', indentLevel * 2); // Adjusted indentation

            // Calculate gradient color
            float t = (float)indentLevel / (GetMaxIndentLevel() - 1);
            Color color = Color.Lerp(new Color(1f, 0f, 0f, 0.2f), new Color(1f, 1f, 0f, 0.2f), t);

            // Draw background rectangle
            Rect rect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight);
            EditorGUI.DrawRect(rect, color);

            // Draw label
            EditorGUI.LabelField(rect, indent + type.typeName);
        }
    }

    private int GetIndentLevel(ProductTypeSo type)
    {
        int level = 0;
        while (type.parentType is not null)
        {
            level++;
            type = type.parentType;
        }
        return level;
    }

    private int GetMaxIndentLevel()
    {
        return availableTypes.Max(type => GetIndentLevel(type));
    }
}