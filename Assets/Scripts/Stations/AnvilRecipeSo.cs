using UnityEngine;

[CreateAssetMenu(fileName = "AnvilRecipe_new", menuName = "ScriptableObject/Recipe/Anvil")]
public class AnvilRecipeSo : ScriptableObject
{
    public ProductSo input;
    public ProductSo output;
    public int hitToProcess;
}
