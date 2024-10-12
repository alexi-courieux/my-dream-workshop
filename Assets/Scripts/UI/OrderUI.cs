using System;
using TMPro;
using UnityEngine;
public class OrderUI : TabsManager
{
    [SerializeField] private TMP_Text moneyText;
    
    
    private new void Start()
    {
        base.Start();
        SetMoneyText();
        EconomyManager.Instance.OnMoneyChanged += EconomyManager_OnMoneyChanged;
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