using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


public abstract class TabsManager : MonoBehaviour
{
    [SerializeField] private GameObject[] tabs;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button[] tabButtons;
    [SerializeField] private Sprite inactiveTabBackground;
    [SerializeField] private Sprite activeTabBackground;

    private int currentTabIndex;
    
    private void Awake() {
        foreach (GameObject go in tabs)
        {
            go.SetActive(false);
        }
        closeButton.onClick.AddListener(Hide);
        for (int i = 0; i < tabButtons.Length; i++)
        {
            int buttonIndex = i;
            tabButtons[i].onClick.AddListener(() => SwitchToTab(buttonIndex));
        }
    }

    protected void Start()
    {
        currentTabIndex = 0;
        SwitchToTab(currentTabIndex);
        Hide();

        InputManager.Instance.OnMenuCancel += (_, _) => Hide();
        InputManager.Instance.OnMenuPrevious += (_, _) => SwitchToTab(currentTabIndex - 1);
        InputManager.Instance.OnMenuNext += (_, _) => SwitchToTab(currentTabIndex + 1);
    }

    public void Show()
    {
        if (gameObject.activeSelf) return;
        InputManager.Instance.DisableGameplayInput();
        InputManager.Instance.EnableMenuInput();
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
        InputManager.Instance.DisableMenuInput();
        InputManager.Instance.EnableGameplayInput();
    }

    private void SwitchToTab(int newIndex)
    {
        if (newIndex < 0) newIndex = tabs.Length - 1;
        if (newIndex >= tabs.Length) newIndex = 0;
        currentTabIndex = newIndex;
        
        for (int i = 0; i < tabButtons.Length; i++)
        {
            tabs[i].SetActive(i == newIndex);
            tabButtons[i].image.sprite = i == newIndex ? activeTabBackground : inactiveTabBackground;
        }
    }
}
