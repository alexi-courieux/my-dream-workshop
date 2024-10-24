using System;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject hasProgressGameObject;
    [SerializeField] private Image barImage;
    [SerializeField] private bool hideOnMinAndMax;

    private IHasProgress hasProgress;

    private void Start() {
        hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
        hasProgress.OnProgressChanged += HasProgress_OnProgressChanged;

        bool stationHandleFocus = hasProgressGameObject.TryGetComponent(out IFocusable focusableStation);
        if (stationHandleFocus)
        {
            focusableStation.OnFocus += FocusableStation_OnFocus;
            focusableStation.OnStopFocus += FocusableStation_OnStopFocus;
        }
        
        barImage.fillAmount = 0f;

        Hide();
    }

    private void HasProgress_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e)
    {
        barImage.fillAmount = e.progressNormalized;

        if (hideOnMinAndMax && e.progressNormalized == 0f || e.progressNormalized == 1f)
        {
            Hide();
        }
        else 
        {
            Show();
        }
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
