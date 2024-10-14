using UnityEngine;

[CreateAssetMenu(fileName = "RecipeBookStation", menuName = "ScriptableObjects/RecipeBookStation")]
public class RecipeBookStationSo : ScriptableObject
{
    public string stationName;
    public RecipeSo recipeTypeSample;
    public Sprite stationSprite;
}