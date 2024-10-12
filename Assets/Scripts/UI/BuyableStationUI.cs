using System;
using TMPro;
using UnityEngine;

public class BuyableStationUI : MonoBehaviour
{
    [SerializeField] private BuyableStation buyableStation;
    [SerializeField] private TMP_Text priceText;

    private void Start()
    {
        buyableStation.OnFocus += BuyableStation_OnFocus;
        buyableStation.OnStopFocus += BuyableStation_OnStopFocus;
        Hide();
    }

    private void OnDestroy()
    {
        buyableStation.OnFocus -= BuyableStation_OnFocus;
        buyableStation.OnStopFocus -= BuyableStation_OnStopFocus;
    }

    private void BuyableStation_OnFocus(object sender, EventArgs e)
    {
        Show();
    }
    
    private void BuyableStation_OnStopFocus(object sender, EventArgs e)
    {
        Hide();
    }
    
    private void Show()
    {
        gameObject.SetActive(true);
        UpdateVisuals();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
    
    private void UpdateVisuals()
    {
        priceText.text = buyableStation.GetPrice().ToString();
    }

}