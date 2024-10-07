using UnityEngine;

[CreateAssetMenu(fileName = "Product", menuName = "ScriptableObject/Product")]
public class ProductSo : ScriptableObject
{
    public Transform prefab;
    public Sprite sprite;
    public string itemName;
    public int buyPrice;
    public int sellPrice;

    public override bool Equals(object other)
    {
        if (other is ProductSo product)
        {
            return product.itemName == itemName;
        }

        return false;
    }
    
    public override int GetHashCode()
    {
        return itemName.GetHashCode();
    }
}