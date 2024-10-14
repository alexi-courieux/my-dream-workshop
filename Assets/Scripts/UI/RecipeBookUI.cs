using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class RecipeBookUI : MonoBehaviour
{
    [SerializeField] private Button closeButton;
    
    [SerializeField] private RecipeBookSingleRecipeUI recipeTemplate;
    [SerializeField] private Transform recipesParent;
    
    [SerializeField] private RecipeBookStationSingleUI stationButtonTemplate;
    [SerializeField] private Transform stationButtonsParent;
    [SerializeField] private RecipeBookStationSo[] stations;
    
    [SerializeField] private TMP_Text currentStationName;

    private int currentStationIndex;
    
    private void Awake()
    {
        closeButton.onClick.AddListener(Hide);
    }

    private void Start()
    { 
        recipeTemplate.gameObject.SetActive(false);
        stationButtonTemplate.gameObject.SetActive(false);
        currentStationIndex = 0;
        CreateStationButtons();
        SelectStation(stations[currentStationIndex]);
        
        Hide();
        
        InputManager.Instance.OnRecipeBook += InputManager_OnRecipeBook;
    }
    
    private void InputManager_OnRecipeBook(object sender, EventArgs e)
    {
        if (gameObject.activeSelf)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }
    
    public void Show()
    {
        if (gameObject.activeSelf) return;
        gameObject.SetActive(true);
        InputManager.Instance.DisableGameplayInput();
        InputManager.Instance.EnableMenuInput();
        
        InputManager.Instance.OnMenuCancel += InputManager_OnMenuCancel;
        InputManager.Instance.OnMenuPrevious += InputManager_OnMenuPrevious;
        InputManager.Instance.OnMenuNext += InputManager_OnMenuNext;
    }
    
    public void Hide()
    {
        gameObject.SetActive(false);
        InputManager.Instance.EnableGameplayInput();
        InputManager.Instance.DisableMenuInput();
        
        InputManager.Instance.OnMenuCancel -= InputManager_OnMenuCancel;
        InputManager.Instance.OnMenuPrevious -= InputManager_OnMenuPrevious;
        InputManager.Instance.OnMenuNext -= InputManager_OnMenuNext;
    }
    
    private void InputManager_OnMenuPrevious(object sender, EventArgs e)
    {
        SelectStation(currentStationIndex - 1);
    }
    
    private void InputManager_OnMenuNext(object sender, EventArgs e)
    {
        SelectStation(currentStationIndex + 1);
    }
    
    private void InputManager_OnMenuCancel(object sender, EventArgs e)
    {
        Hide();
    }
    
    private void CreateStationButtons()
    {
        foreach (RecipeBookStationSo station in stations)
        {
            RecipeBookStationSingleUI stationUI = Instantiate(stationButtonTemplate, stationButtonsParent);
            stationUI.gameObject.SetActive(true);
            stationUI.UpdateVisuals(station, this);
        }
    }

    private void SelectStation(int index)
    {
        if (index < 0) index = stations.Length - 1;
        if (index >= stations.Length) index = 0;
        SelectStation(stations[index]);;
    }
    
    public void SelectStation(RecipeBookStationSo selectedStation)
    {
        currentStationIndex = Array.IndexOf(stations, selectedStation);
        currentStationName.text = selectedStation.stationName;
        
        foreach (Transform child in recipesParent)
        {
            if (child == recipeTemplate.transform) continue;
            
            Destroy(child.gameObject);
        }
        
        foreach (RecipeSo recipe in RecipeManager.Instance.GetRecipes())
        {
            if (recipe.GetType() != selectedStation.recipeTypeSample.GetType()) continue;
            
            RecipeBookSingleRecipeUI recipeUI = Instantiate(recipeTemplate, recipesParent);
            recipeUI.gameObject.SetActive(true);
            recipeUI.UpdateVisuals(recipe);
        }
        
        foreach (Transform child in stationButtonsParent)
        {
            RecipeBookStationSingleUI stationUI = child.GetComponent<RecipeBookStationSingleUI>();
            stationUI.CheckActive(selectedStation);
        }
    }
}