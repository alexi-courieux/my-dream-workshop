using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class InventorySlotUI : MonoBehaviour
{
    [SerializeField] private Image productImage;
    [SerializeField] private TMP_Text amountText;
    [SerializeField] private Transform defaultBackground;
    [SerializeField] private Transform selectedBackground;
    
    private SlotInventoryItem<ItemSo> item;

    public void SetItem(SlotInventoryItem<ItemSo> i)
    {
        item = i;
    }
    
    public void UpdateVisuals(int slotIndex)
    {
        bool selected = slotIndex == Player.Instance.HandleSystem.SelectedSlotIndex;
        defaultBackground.gameObject.SetActive(!selected);
        selectedBackground.gameObject.SetActive(selected);
        
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