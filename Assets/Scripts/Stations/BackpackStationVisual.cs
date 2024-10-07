using System;
using UnityEngine;

namespace AshLight.BakerySim.Stations
{
    public class BackpackStationVisual : MonoBehaviour
    {
        [SerializeField] private BackpackStation backpackStation;
        [SerializeField] private GameObject backpackVisual;

        private void Start()
        {
            backpackStation.OnBackpackPut += (sender, args) => Show();
            backpackStation.OnBackpackTake += (sender, args) => Hide();
            Show();
        }

        private void Show()
        {
            backpackVisual.SetActive(true);
        }
        
        private void Hide()
        {
            backpackVisual.SetActive(false);
        }
    }
}