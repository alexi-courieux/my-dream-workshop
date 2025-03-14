using System;
using TMPro;
using UnityEngine;
public class OrderUI : TabsManager
{
    [SerializeField] private TMP_Text moneyText;
    [SerializeField] private OrderStation orderStation;
    
    private new void Start()
    {
        base.Start();
        SetMoneyText();
        EconomyManager.Instance.OnMoneyChanged += EconomyManager_OnMoneyChanged;
        orderStation.OnUse += OrderStation_OnUse;
    }
    
    private void OrderStation_OnUse(object sender, EventArgs e)
    {
        Show();
    }
    
    private void EconomyManager_OnMoneyChanged(object sender, EventArgs e)
    {
        SetMoneyText();
    }
    
    private void SetMoneyText()
    {
        moneyText.text = EconomyManager.Instance.GetMoney().ToString("D");
    }
}