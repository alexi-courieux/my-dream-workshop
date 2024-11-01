using UnityEngine;

public abstract class TutorialStep : MonoBehaviour
{
    [SerializeField] private string tutorialName;
    [SerializeField] private string tutorialText;
    
    public abstract void Show();
    public abstract void Hide();
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
}