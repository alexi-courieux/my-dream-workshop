using UnityEngine;

[CreateAssetMenu(fileName = "recipeGroup_new", menuName = "ScriptableObject/Recipe/BuyableRecipeGroup", order = 0)]
public class BuyableRecipeGroupSo : ScriptableObject
{
    public RecipeSo[] recipes;
    public int price;
    public string groupName;
}
