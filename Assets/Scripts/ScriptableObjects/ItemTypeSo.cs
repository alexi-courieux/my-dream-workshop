using UnityEngine;

[CreateAssetMenu(fileName = "ItemType", menuName = "ScriptableObject/ItemType")]
public class ItemTypeSo : ScriptableObject
{
    public string typeName;
    public ItemTypeSo parentType;
    public bool assignable = true;
    
    public bool IsType(ItemTypeSo type)
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