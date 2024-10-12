using UnityEngine;

[CreateAssetMenu(fileName = "AnvilRecipe_new", menuName = "ScriptableObject/Recipe/Anvil")]
public class AnvilRecipeSo : RecipeSo
{
    public ProductSo input;
    public int hitToProcess;
}
