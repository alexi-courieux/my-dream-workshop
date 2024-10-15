using UnityEngine;

public abstract class TutorialStep : MonoBehaviour
{
    [SerializeField] protected TutorialUI tutorialUI;

    public abstract void Show();
}