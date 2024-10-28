using UnityEngine;

[CreateAssetMenu(fileName = "Product", menuName = "ScriptableObject/FinalProduct")]
public class FinalProductSo : ProductSo
{
    public enum ItemType {
        Helmet,
        Chest,
        Pants,
        Boot,
        Weapon
    }
    public ItemType itemType;

    public override bool Equals(object other)
    {
        if (other is FinalProductSo finalProduct)
        {
            return finalProduct.itemName == itemName;
        }

        return false;
    }
    
    public override int GetHashCode()
    {
        return itemName.GetHashCode();
    }
}
