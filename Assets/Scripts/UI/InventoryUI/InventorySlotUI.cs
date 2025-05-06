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
    [SerializeField] private Transform defaultCountBackground;
    [SerializeField] private Transform selectedCountBackground;
    
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
            defaultCountBackground.gameObject.SetActive(false);
            selectedCountBackground.gameObject.SetActive(false);
            return;
        }
        
        productImage.enabled = true;
        productImage.sprite = item.Item.sprite;
        
        bool hasAmount = item.Amount > 1;
        amountText.enabled = hasAmount;
        if (hasAmount)
        {
            defaultCountBackground.gameObject.SetActive(true);
            if (!defaultCountBackground.Equals(selectedCountBackground))
            {
                defaultCountBackground.gameObject.SetActive(!selected);
                selectedCountBackground.gameObject.SetActive(selected);
            }
            amountText.text = $"{item.Amount:D}";
        }
        else
        {
            defaultCountBackground.gameObject.SetActive(false);
            selectedCountBackground.gameObject.SetActive(false);
        }
    }
}