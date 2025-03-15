using System;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
public class ChestUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countText;
    [FormerlySerializedAs("singleChestStation")] [FormerlySerializedAs("chestStation")] [SerializeField] private SingleItemChestStation singleItemChestStation;
    [SerializeField] private GameObject countTransform;
    [SerializeField] private Image productImage;

    private void Start()
    {
        singleItemChestStation.OnFocus += ((sender, args) => ShowCount());
        singleItemChestStation.OnStopFocus += ((sender, args) => HideCount());
        singleItemChestStation.OnProductAmountChanged += ((sender, args) => UpdateVisuals());
        productImage.sprite = singleItemChestStation.GetItemSo().sprite;
        HideCount();
    }

    private void ShowCount()
    {
        UpdateVisuals();
        countTransform.SetActive(true);
    }

    private void HideCount()
    {
        countTransform.SetActive(false);
    }
    
    private void UpdateVisuals()
    {
        countText.text = singleItemChestStation.GetProductAmount().ToString("D");
    }
}