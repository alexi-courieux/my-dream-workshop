using UnityEngine;

[CreateAssetMenu(fileName = "recipeList", menuName = "ScriptableObject/Recipe/RecipeList", order = 0)]
public class RecipeListSo : ScriptableObject
{
    public RecipeSo[] recipes;
}
