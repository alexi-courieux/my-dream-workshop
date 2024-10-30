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
    
    private SlotInventoryItem<ProductSo> item;

    private void Start()
    {
        defaultBackground.gameObject.SetActive(true);
        selectedBackground.gameObject.SetActive(false);
    }

    public void SetItem(SlotInventoryItem<ProductSo> i)
    {
        item = i;
    }
    
    public void SetSelected(bool selected)
    {
        defaultBackground.gameObject.SetActive(!selected);
        selectedBackground.gameObject.SetActive(selected);
    }
    
    public void UpdateVisuals(int index)
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
        
        SetSelected(index == Player.Instance.HandleSystem.SelectedSlotIndex);
    }
}