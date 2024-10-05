using UnityEngine;

[CreateAssetMenu(fileName = "SculptingRecipe_new", menuName = "ScriptableObject/Recipe/Sculpting")]
public class SculptingRecipeSo : ScriptableObject
{
    public ProductSo input;
    public ProductSo output;
    public int hitToProcess;

}
