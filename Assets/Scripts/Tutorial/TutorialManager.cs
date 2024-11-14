using System;
using UnityEngine;

public struct TutorialStepStartEvent
{
    public string TutorialName;
    public string TutorialText;
    public Transform Objective;
    public Vector3 PointerOffset;
}
public class TutorialManager : MonoBehaviour
{ 
    public static TutorialManager Instance { get; private set; }

    public event EventHandler<TutorialStepStartEvent> OnStepStart;
    public event EventHandler OnStepEnd;
    
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
        step.Initialise();
        OnStepStart?.Invoke(this, new TutorialStepStartEvent
        {
            TutorialName = step.GetTutorialName(),
            TutorialText = step.GetTutorialText(),
            Objective = step.GetObjective(),
            PointerOffset = step.GetPointerOffset()
        });
    }
    
    public void CompleteTutorialStep()
    {
        OnStepEnd?.Invoke(this, EventArgs.Empty);
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