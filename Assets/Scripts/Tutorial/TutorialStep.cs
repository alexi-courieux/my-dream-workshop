using UnityEngine;

public abstract class TutorialStep : MonoBehaviour
{
    [SerializeField] private string tutorialName;
    [SerializeField] private string tutorialText;
    [SerializeField] private Transform objective;
    
    public abstract void Initialise();
    protected void Complete()
    {
        TutorialManager.Instance.CompleteTutorialStep();
        Destroy(this);
    }
    
    public string GetTutorialName()
    {
        return tutorialName;
    }
    
    public string GetTutorialText()
    {
        return tutorialText;
    }
    
    public Transform GetObjective()
    {
        return objective;
    }
}