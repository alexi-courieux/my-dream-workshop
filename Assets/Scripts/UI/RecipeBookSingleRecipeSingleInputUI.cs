using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeBookSingleRecipeSingleInputUI : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private TMP_Text amount;
    
    public void UpdateVisuals(ProductSo product, int quantity)
    {
        icon.sprite = product.sprite;
        amount.text = $"x{quantity:D}";
    }
}