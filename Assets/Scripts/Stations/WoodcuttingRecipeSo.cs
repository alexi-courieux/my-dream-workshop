using UnityEngine;

[CreateAssetMenu(fileName = "WoodcuttingRecipe_new", menuName = "ScriptableObject/Recipe/Woodcutting")]
public class WoodcuttingRecipeSo : RecipeSo
{
    public ProductSo input;
    public float timeToProcess;

}
