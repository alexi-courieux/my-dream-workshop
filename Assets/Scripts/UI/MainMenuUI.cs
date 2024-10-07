using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButtonNathan;
    [SerializeField] private Button playButtonAlexi;
    [SerializeField] private Button quitButton;

    private void Awake() {
        playButtonNathan.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.NathanScene);
        });
        playButtonAlexi.onClick.AddListener(() => {
            Loader.Load(Loader.Scene.AlexiScene);
        });
        quitButton.onClick.AddListener(() => {
            Application.Quit();
        });
    }
}
