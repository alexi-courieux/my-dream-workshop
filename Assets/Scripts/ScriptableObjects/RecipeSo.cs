using UnityEngine;
using UnityEngine.Serialization;
public abstract class RecipeSo : ScriptableObject
{
    public ProductSo output;
    public ProductSo[] inputs;
    public int buyPrice;
}