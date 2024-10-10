using UnityEngine;

[CreateAssetMenu(fileName = "CastingRecipe_new", menuName = "ScriptableObject/Recipe/Casting")]
public class CastingRecipeSo : ScriptableObject
{
    public ProductSo input;
    public ProductSo output;
    public float timeToProcess;
}
