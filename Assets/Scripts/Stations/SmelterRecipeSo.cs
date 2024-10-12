using UnityEngine;

[CreateAssetMenu(fileName = "SmelterRecipe_new", menuName = "ScriptableObject/Recipe/Smelter")]
public class SmelterRecipeSo : RecipeSo
{
    public ProductSo input;
    public float timeToProcess;
}
