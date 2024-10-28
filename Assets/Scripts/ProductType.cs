using UnityEngine;

[CreateAssetMenu(fileName = "ProductType", menuName = "ScriptableObject/ProductType")]
public class ProductType : ScriptableObject
{
    public string typeName;
    public ProductType parentType;
    public bool assignable = true;
    
    public bool IsType(ProductType type)
    {
        if (type == this) return true;
        if (parentType is null) return false;
        
        return parentType.IsType(type);
    }
    
    public string GetPath()
    {
        if (parentType is null) return typeName;
        
        return parentType.GetPath() + "/" + typeName;
    }
}