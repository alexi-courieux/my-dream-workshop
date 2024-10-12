using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


public abstract class TabsManager : MonoBehaviour
{
    [SerializeField] private GameObject[] tabs;
    [SerializeField] private Button closeButton;
    [SerializeField] private Button[] tabButtons;
    [SerializeField] private Image[] tabButtonBackgroundImages;
    [SerializeField] private Sprite inactiveTabBackground;
    [SerializeField] private Sprite activeTabBackground;

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
        SwitchToTab(0);
        Hide();
    }

    public void Show()
    {
        if (gameObject.activeSelf) return;
        InputManager.Instance.DisableGameplayInput();
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
        InputManager.Instance.EnableGameplayInput();
    }

    private void SwitchToTab(int tabID)
    {
        for (int i = 0; i < tabButtons.Length; i++)
        {
            tabs[i].SetActive(i == tabID);
            tabButtonBackgroundImages[i].sprite = i == tabID ? activeTabBackground : inactiveTabBackground;
        }
    }
}
