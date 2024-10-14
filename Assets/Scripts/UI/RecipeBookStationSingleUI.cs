using UnityEngine;
using UnityEngine.UI;

public class RecipeBookStationSingleUI : MonoBehaviour
{
    [SerializeField] private RecipeBookStationSo station;
    [SerializeField] private Image icon;
    [SerializeField] private Button button;
    [SerializeField] private RecipeBookUI recipeBookUI;
    
    [SerializeField] private Image background;
    [SerializeField] private Sprite activeTabBackground;
    [SerializeField] private Sprite inactiveTabBackground;
    
    private void Awake()
    {
        button.onClick.AddListener(() =>
        {
            recipeBookUI.SelectStation(station);
        });
    }
    
    public void UpdateVisuals(RecipeBookStationSo station, RecipeBookUI recipeBookUI)
    {
        this.station = station;
        this.recipeBookUI = recipeBookUI;
        icon.sprite = station.stationSprite;
    }
    
    public void CheckActive(RecipeBookStationSo activeStation)
    {
        background.sprite = station == activeStation ? activeTabBackground : inactiveTabBackground;
    }
}