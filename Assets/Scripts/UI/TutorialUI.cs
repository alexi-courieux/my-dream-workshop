using System;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private TMP_Text tutorialNameText;
    [SerializeField] private TMP_Text tutorialDescText;
    [SerializeField] private GameObject tutorialTextObject;

    private void Start()
    {
        tutorialTextObject.SetActive(false);
        TutorialManager.Instance.OnStepStart += TutorialManager_OnStepStart;
    }

    private void TutorialManager_OnStepStart(object sender, TutorialStepStartEvent e)
    {
        Show(e.TutorialName, e.TutorialText);
    }

    private void Show(string tutorialName, string tutorialDesc)
    {
        tutorialTextObject.SetActive(true);
        tutorialNameText.text = tutorialName;
        tutorialDescText.text = tutorialDesc;
    }
    
    private void Hide()
    {
        
    }
}