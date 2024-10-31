using System;
using UnityEngine;

[CreateAssetMenu(fileName = "RecipeBookStation", menuName = "ScriptableObject/RecipeBookStation")]
public class RecipeBookStationSo : ScriptableObject
{
    public string stationName;
    public RecipeSo recipeTypeSample;
    public Sprite stationSprite;
    
    public Type RecipeType => recipeTypeSample.GetType();
}