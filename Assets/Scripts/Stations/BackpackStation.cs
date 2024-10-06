using System;
using UnityEngine;

public class BackpackStation : MonoBehaviour, IInteractable
{
    public event EventHandler OnBackpackPut;
    public event EventHandler OnBackpackTake;
    
    private bool stationHasBackpack;

    private void Start()
    {
        stationHasBackpack = true;
    }
    public void Interact()
    {
        if (stationHasBackpack)
        {
            OnBackpackTake?.Invoke(this, EventArgs.Empty);
            stationHasBackpack = false;
            Player.Instance.HandleSystem.EquipBackpack();
        }
        else
        {
            OnBackpackPut?.Invoke(this, EventArgs.Empty);
            stationHasBackpack = true;
            Player.Instance.HandleSystem.UnequipBackpack();
        }
    }
}