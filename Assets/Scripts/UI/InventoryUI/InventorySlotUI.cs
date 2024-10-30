using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image productImage;
    [SerializeField] private TMP_Text amountText;
    
    private SlotInventoryItem<ProductSo> item;
    
    public void SetItem(SlotInventoryItem<ProductSo> i)
    {
        item = i;
    }
    
    public void UpdateVisuals()
    {
        if (item is null)
        {
            productImage.enabled = false;
            amountText.enabled = false;
            return;
        }
        
        productImage.enabled = true;
        productImage.sprite = item.Item.sprite;
        amountText.enabled = item.Amount > 1;
        amountText.text = $"{item.Amount:D}";
    }
}