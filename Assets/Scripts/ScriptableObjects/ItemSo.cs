using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Item_new", menuName = "ScriptableObject/Item")]
public class ItemSo : ScriptableObject
{
    public string id;
    public Transform prefab;
    public Sprite sprite;
    public ItemTypeSo[] types;
    
    public bool IsType(ItemTypeSo type)
    {
        return types.Any(t => t.IsType(type));
    }
    
    public override bool Equals(object other)
    {
        if (other is ProductSo product)
        {
            return product.id == id;
        }

        return false;
    }
    
    public override int GetHashCode()
    {
        return id.GetHashCode();
    }
}

[CustomEditor(typeof(ItemSo), true)]
public class ItemSoEditor : Editor
{
    private List<ItemTypeSo> availableTypes;
    private bool[] selectedTypes = Array.Empty<bool>();

    private void OnEnable()
    {
        string[] typesGuids = AssetDatabase.FindAssets($"t:{nameof(ItemTypeSo)}");
        availableTypes = typesGuids
            .Select(guid => AssetDatabase.LoadAssetAtPath<ItemTypeSo>(AssetDatabase.GUIDToAssetPath(guid)))
            .Where(type => type.assignable)
            .OrderBy(type => type.GetPath())
            .ToList();
        ProductSo productSo = (ProductSo)target;
        selectedTypes = availableTypes.Select(type => productSo.types.Contains(type)).ToArray();
    }

    public override void OnInspectorGUI()
    {
        // Get the target object
        ItemSo itemSo = (ProductSo)target;

        // Draw default inspector
        DrawDefaultInspector();

        // Display dropdown list for selecting tags
        EditorGUILayout.LabelField($"Available Tags ({availableTypes.Count}):");

        HashSet<ItemTypeSo> displayedParents = new HashSet<ItemTypeSo>();
        for (int i = 0; i < availableTypes.Count; i++)
        {
            if (availableTypes[i].parentType is not null && !displayedParents.Contains(availableTypes[i].parentType))
            {
                DisplayParentHierarchy(availableTypes[i].parentType, displayedParents);
            }

            int indentLevel = GetIndentLevel(availableTypes[i]);
            string indent = new string(' ', Math.Max(indentLevel * 2 - 2, 0)); // Adjusted indentation
            string label = indent + availableTypes[i].typeName;
            if (indentLevel is 0)
            {
                // Assignable with no parent
                Color color = new Color(1f, 0f, 0f, 0.2f);
                Rect rect = EditorGUILayout.GetControlRect(false, EditorGUIUtility.singleLineHeight);
                EditorGUI.DrawRect(rect, color);
                EditorGUI.LabelField(rect, label);
            }
            bool newSelected = EditorGUILayout.ToggleLeft(label, selectedTypes[i]);

            if (newSelected == selectedTypes[i]) continue;

            selectedTypes[i] = newSelected;
            List<ItemTypeSo> selectedProductTypes = new List<ItemTypeSo>();
            for (int j = 0; j < selectedTypes.Length; j++)
            {
                if (selectedTypes[j])
                {
                    selectedProductTypes.Add(availableTypes[j]);
                }
            }
            itemSo.types = selectedProductTypes.ToArray();

            // Apply changes and save asset
            EditorUtility.SetDirty(itemSo);
            AssetDatabase.SaveAssets();
        }
    }

    private void DisplayParentHierarchy(ItemTypeSo parentType, HashSet<ItemTypeSo> displayedParents)
    {
        if (parentType is null || displayedParents.Contains(parentType)) return;

        List<ItemTypeSo> hierarchy = new List<ItemTypeSo>();
        while (parentType is not null && !displayedParents.Contains(parentType))
        {
            hierarchy.Add(parentType);
            displayedParents.Add(parentType);
            parentType = parentType.parentType;
        }

        hierarchy.Reverse();
        for (int i = 0; i < hierarchy.Count; i++)
        {
            ItemTypeSo type = hierarchy[i];
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

    private int GetIndentLevel(ItemTypeSo type)
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