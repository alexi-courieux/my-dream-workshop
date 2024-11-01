using System;
using UnityEngine;

public struct TutorialStepStartEvent
{
    public string TutorialName;
    public string TutorialText;
}
public class TutorialManager : MonoBehaviour
{ 
    public static TutorialManager Instance { get; private set; }

    public event EventHandler<TutorialStepStartEvent> OnStepStart;
    
    [SerializeField] private GameObject keyTutorial;
    [SerializeField] private TutorialStep[] tutorialSteps;
    private int tutorialStepIndex;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        tutorialStepIndex = 0;
        foreach (TutorialStep step in tutorialSteps)
        {
            step.Hide();
            step.gameObject.SetActive(false);
        }
        keyTutorial.SetActive(true);
        InputManager.Instance.OnInteract += InputManager_OnInteract;
        CustomerManager.Instance.DisableSpawning();
    }
    
    private void OnDestroy()
    {
        InputManager.Instance.OnInteract -= InputManager_OnInteract;
    }

    private void InputManager_OnInteract(object sender, EventArgs e)
    {
        keyTutorial.SetActive(false);
        InputManager.Instance.OnInteract -= InputManager_OnInteract;
        SetupTutorialStep();
    }

    public void SetupTutorialStep()
    {
        TutorialStep step = tutorialSteps[tutorialStepIndex];
        OnStepStart?.Invoke(this, new TutorialStepStartEvent
        {
            TutorialName = step.GetTutorialName(),
            TutorialText = step.GetTutorialText()
        });
        step.Show();
    }
    
    public void CompleteTutorialStep()
    {
        tutorialStepIndex++;
        if (tutorialStepIndex < tutorialSteps.Length)
        {
            SetupTutorialStep();
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}