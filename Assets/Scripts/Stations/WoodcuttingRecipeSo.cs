using UnityEngine;

[CreateAssetMenu(fileName = "WoodcuttingRecipe_new", menuName = "ScriptableObject/Recipe/Woodcutting")]
public class WoodcuttingRecipeSo : ScriptableObject
{
    public ProductSo input;
    public ProductSo output;
    public float timeToProcess;

}
