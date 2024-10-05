using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class ChestUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countText;
    [SerializeField] private ChestStation chestStation;
    [SerializeField] private GameObject countTransform;
    [SerializeField] private Image productImage;

    private void Start()
    {
        chestStation.OnFocus += ((sender, args) => ShowCount());
        chestStation.OnStopFocus += ((sender, args) => HideCount());
        chestStation.OnProductAmountChanged += ((sender, args) => UpdateVisuals());
        productImage.sprite = chestStation.GetProductSo().sprite;
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
        countText.text = chestStation.GetProductAmount().ToString("D");
    }
}