using System;
using TMPro;
using UnityEngine;
public class EconomyUI : MonoBehaviour
{
    [SerializeField] private TMP_Text moneyText;

    private void Start()
    {
        EconomyManager.Instance.OnMoneyChanged += EconomyManager_OnMoneyChanged;
        UpdateMoneyText();
    }
    
    private void OnDestroy()
    {
        EconomyManager.Instance.OnMoneyChanged -= EconomyManager_OnMoneyChanged;
    }
    
    private void EconomyManager_OnMoneyChanged(object sender, EventArgs e)
    {
        UpdateMoneyText();
    }
    
    private void UpdateMoneyText()
    {
        moneyText.text = $"{EconomyManager.Instance.GetMoney():D}";
    }
}