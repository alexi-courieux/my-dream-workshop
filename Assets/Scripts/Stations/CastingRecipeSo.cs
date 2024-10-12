using UnityEngine;

[CreateAssetMenu(fileName = "CastingRecipe_new", menuName = "ScriptableObject/Recipe/Casting")]
public class CastingRecipeSo : RecipeSo
{
    public ProductSo input;
    public float timeToProcess;
}
