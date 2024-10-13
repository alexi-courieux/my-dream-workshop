using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public abstract class TabsManager : MonoBehaviour
{
    [SerializeField] private GameObject[] tabs;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button[] tabButtons;
    [SerializeField] private Sprite inactiveTabBackground;
    [SerializeField] private Sprite activeTabBackground;

    private int currentTabIndex;
    private ITabElement[] tabElements;
    
    private void Awake() {
        closeButton.onClick.AddListener(Hide);
        for (int i = 0; i < tabButtons.Length; i++)
        {
            int buttonIndex = i;
            tabButtons[i].onClick.AddListener(() => SwitchToTab(buttonIndex));
        }
        tabElements = new ITabElement[tabs.Length];
        
        for (int i = 0; i < tabs.Length; i++)
        {
            if (tabs[i].TryGetComponent(out ITabElement tab))
            {
                tabElements[i] = tab;
            }
            else
            {
                throw new Exception($"Tab {tabs[i].name} does not implement ITabElement");
            }
        }
    }

    protected void Start()
    {
        currentTabIndex = 0;
        Hide();

        InputManager.Instance.OnMenuCancel += (_, _) => Hide();
        InputManager.Instance.OnMenuPrevious += (_, _) => SwitchToTab(currentTabIndex - 1);
        InputManager.Instance.OnMenuNext += (_, _) => SwitchToTab(currentTabIndex + 1);
    }

    public void Show()
    {
        if (gameObject.activeSelf) return;
        gameObject.SetActive(true);
        InputManager.Instance.DisableGameplayInput();
        InputManager.Instance.EnableMenuInput();
        SwitchToTab(currentTabIndex);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
        InputManager.Instance.DisableMenuInput();
        EventSystem.current.SetSelectedGameObject(null);
        InputManager.Instance.EnableGameplayInput();
    }

    private void SwitchToTab(int newIndex)
    {
        if (newIndex < 0) newIndex = tabs.Length - 1;
        if (newIndex >= tabs.Length) newIndex = 0;
        currentTabIndex = newIndex;
        
        for (int i = 0; i < tabButtons.Length; i++)
        {
            if (i == newIndex)
            {
                tabElements[i].Show();
            } else {
                tabElements[i].Hide();
            }
            tabButtons[i].image.sprite = i == newIndex ? activeTabBackground : inactiveTabBackground;
        }
    }
}
