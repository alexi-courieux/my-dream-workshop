using System;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{
    [SerializeField] private GameObject hasProgressGameObject;
    [SerializeField] private Image barImage;
    [SerializeField] private bool hideOnMinAndMax;

    private IHasProgress hasProgress;
    private float progress;

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
        progress = e.progressNormalized;
        barImage.fillAmount = progress;

        Show();
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
        if (hideOnMinAndMax && (progress.Equals(0f) || progress.Equals(1f)))
        {
            Hide();
            return;
        }
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
