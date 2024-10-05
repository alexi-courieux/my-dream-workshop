using UnityEngine;

[CreateAssetMenu(fileName = "AssemblyRecipe_new", menuName = "ScriptableObject/Recipe/Assembly")]
public class AssemblyRecipeSo : ScriptableObject
{
    public ProductSo[] inputs;
    public ProductSo output;
    public int hitToProcess;
}
