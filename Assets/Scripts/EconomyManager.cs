using System;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    private const int StartingMoney = 100;
    
    public static EconomyManager Instance { get; private set; }
    
    public event EventHandler OnMoneyChanged;
    
    private int money;

    private void Awake()
    {
        Instance = this;
        money = StartingMoney;
    }
    
    public void AddMoney(int amount)
    {
        money += amount;
        OnMoneyChanged?.Invoke(this, EventArgs.Empty);
    }
    
    public void RemoveMoney(int amount)
    {
        money -= amount;
        OnMoneyChanged?.Invoke(this, EventArgs.Empty);
    }
    
    public int GetMoney()
    {
        return money;
    }
}