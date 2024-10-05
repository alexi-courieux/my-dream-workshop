using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "ProductsList", menuName = "ScriptableObject/ProductsDictionary", order = 0)]
public class ProductDictionarySo : ScriptableObject
{
    public ProductSo[] products;
}

#if UNITY_EDITOR
[CustomEditor(typeof(ProductDictionarySo))]
public class SellableItemDictionarySoEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        ProductDictionarySo itemDictionary = (ProductDictionarySo) target;
        if (GUILayout.Button("Autofill"))
        {
            string[] productsGuids = AssetDatabase.FindAssets($"t:{nameof(ProductSo)}");
            itemDictionary.products = productsGuids
                .Select(guid => AssetDatabase.LoadAssetAtPath<ProductSo>(AssetDatabase.GUIDToAssetPath(guid)))
                .ToArray();
            AssetDatabase.SaveAssets();
        }
    }
}
#endif