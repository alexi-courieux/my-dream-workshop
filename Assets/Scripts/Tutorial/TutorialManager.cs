using System;
using System.Collections;
using UnityEngine;

public struct TutorialStepStartEvent
{
    public string TutorialName;
    public string TutorialText;
    public Transform? Objective;
}
public class TutorialManager : MonoBehaviour
{ 
    public static TutorialManager Instance { get; private set; }

    public event EventHandler<TutorialStepStartEvent> OnStepStart;
    public event EventHandler OnStepEnd;
    
    [SerializeField] private GameObject keyTutorial;
    [SerializeField] private TutorialStep[] tutorialSteps;
    [SerializeField] private float tutorialStepDelay = 0.8f;
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
            TutorialText = step.GetTutorialText(),
            Objective = step.GetObjective()
        });
        step.Show();
    }
    
    public void CompleteTutorialStep()
    {
        OnStepEnd?.Invoke(this, EventArgs.Empty);
        tutorialStepIndex++;
        if (tutorialStepIndex < tutorialSteps.Length)
        {
            StartCoroutine(StartNextStep());
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    
    private IEnumerator StartNextStep()
    {
        yield return new WaitForSeconds(tutorialStepDelay);
        SetupTutorialStep();
    }
}