using UnityEngine;

[CreateAssetMenu(fileName = "AssemblyRecipe_new", menuName = "ScriptableObject/Recipe/Assembly")]
public class AssemblyRecipeSo : RecipeSo
{
    public ProductSo[] inputs;
    public int hitToProcess;
}
