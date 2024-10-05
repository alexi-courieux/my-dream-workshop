﻿using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipes", menuName = "ScriptableObject/Recipe/_RecipeDictionary", order = 0)]
public class RecipesDictionarySo : ScriptableObject
{
    public OvenRecipeSo[] ovenRecipes;
    public BlenderRecipeSo[] blenderRecipes;
    public ToolRecipeSo[] toolRecipes;
    public WoodcuttingRecipeSo[] woodcuttingRecipes;
    public SmelterRecipeSo[] smelterRecipeSo;
    public MoldingRecipeSo[] moldingRecipeSo;
    public AnvilRecipeSo[] anvilRecipeSo;
    public SculptingRecipeSo[] sculptingRecipeSo;
    public AssemblyRecipeSo[] assemblyRecipeSo;
}

#if UNITY_EDITOR
[CustomEditor(typeof(RecipesDictionarySo))]
public class RecipeDictionarySoEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        RecipesDictionarySo recipes = (RecipesDictionarySo) target;
        if (GUILayout.Button("Autofill"))
        {
            string[] recipeGuids = AssetDatabase.FindAssets($"t:{nameof(OvenRecipeSo)}");
            recipes.ovenRecipes = recipeGuids
                .Select(guid => AssetDatabase.LoadAssetAtPath<OvenRecipeSo>(AssetDatabase.GUIDToAssetPath(guid)))
                .ToArray();

            recipeGuids = AssetDatabase.FindAssets($"t:{nameof(BlenderRecipeSo)}");
            recipes.blenderRecipes = recipeGuids
                .Select(guid => AssetDatabase.LoadAssetAtPath<BlenderRecipeSo>(AssetDatabase.GUIDToAssetPath(guid)))
                .ToArray();

            recipeGuids = AssetDatabase.FindAssets($"t:{nameof(ToolRecipeSo)}");
            recipes.toolRecipes = recipeGuids
                .Select(guid => AssetDatabase.LoadAssetAtPath<ToolRecipeSo>(AssetDatabase.GUIDToAssetPath(guid)))
                .ToArray();

            recipeGuids = AssetDatabase.FindAssets($"t:{nameof(WoodcuttingRecipeSo)}");
            recipes.woodcuttingRecipes = recipeGuids
                .Select(guid => AssetDatabase.LoadAssetAtPath<WoodcuttingRecipeSo>(AssetDatabase.GUIDToAssetPath(guid)))
                .ToArray();

            recipeGuids = AssetDatabase.FindAssets($"t:{nameof(SmelterRecipeSo)}");
            recipes.smelterRecipeSo = recipeGuids
                .Select(guid => AssetDatabase.LoadAssetAtPath<SmelterRecipeSo>(AssetDatabase.GUIDToAssetPath(guid)))
                .ToArray();

            recipeGuids = AssetDatabase.FindAssets($"t:{nameof(MoldingRecipeSo)}");
            recipes.moldingRecipeSo = recipeGuids
                .Select(guid => AssetDatabase.LoadAssetAtPath<MoldingRecipeSo>(AssetDatabase.GUIDToAssetPath(guid)))
                .ToArray();

            recipeGuids = AssetDatabase.FindAssets($"t:{nameof(AnvilRecipeSo)}");
            recipes.anvilRecipeSo = recipeGuids
                .Select(guid => AssetDatabase.LoadAssetAtPath<AnvilRecipeSo>(AssetDatabase.GUIDToAssetPath(guid)))
                .ToArray();

            recipeGuids = AssetDatabase.FindAssets($"t:{nameof(SculptingRecipeSo)}");
            recipes.sculptingRecipeSo = recipeGuids
                .Select(guid => AssetDatabase.LoadAssetAtPath<SculptingRecipeSo>(AssetDatabase.GUIDToAssetPath(guid)))
                .ToArray();

            recipeGuids = AssetDatabase.FindAssets($"t:{nameof(AssemblyRecipeSo)}");
            recipes.assemblyRecipeSo = recipeGuids
                .Select(guid => AssetDatabase.LoadAssetAtPath<AssemblyRecipeSo>(AssetDatabase.GUIDToAssetPath(guid)))
                .ToArray();

            AssetDatabase.SaveAssets();
        }
    }
}
#endif