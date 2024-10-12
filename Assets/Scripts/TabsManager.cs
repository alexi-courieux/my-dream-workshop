using UnityEngine;
using UnityEngine.UI;


public class TabsManager : MonoBehaviour
{
    [SerializeField] private GameObject[] tabs;
    [SerializeField] private Button closeButton;
    [SerializeField] private Image[] tabButtons;
    [SerializeField] private Sprite inactiveTabBackground;
    [SerializeField] private Sprite activeTabBackground;

    private void Awake() {
        foreach (GameObject gameObject in tabs)
        {
            gameObject.SetActive(false);
        }
        closeButton.onClick.AddListener(Hide);
    }

    private void Start()
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
        foreach (GameObject gameObject in tabs)
        {
            gameObject.SetActive(false);
        }
        tabs[tabID].SetActive(true);

        foreach (Image image in tabButtons)
        {
            image.sprite = inactiveTabBackground;
        }
        tabButtons[tabID].sprite = activeTabBackground;
    }

}
