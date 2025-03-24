using System;
using UnityEngine;
using UnityEngine.UI;

namespace AshLight.BakerySim.UI
{
    public class OrderStationInputsUI : MonoBehaviour
    {
        [SerializeField] GameObject station;
        
        private void Awake()
        {
            if (station.TryGetComponent(out IFocusable focusableStation))
            {
                focusableStation.OnFocus += FocusableStation_OnFocus;
                focusableStation.OnStopFocus += FocusableStation_OnStopFocus;
            }
            else
            {
                Debug.LogError("Station doesn't Implement IFocusable");
            }
        }

        private void Start()
        {
            Hide();
        }

        private void FocusableStation_OnFocus(object sender, EventArgs e)
        {
            Show();
        }
    
        private void FocusableStation_OnStopFocus(object sender, EventArgs e)
        {
            Hide();
        }

        private void Show()
        {
            gameObject.SetActive(true);
        }
        private void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
