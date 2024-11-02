using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private TMP_Text tutorialNameText;
    [SerializeField] private TMP_Text tutorialDescText;
    [SerializeField] private GameObject tutorialPanel;
    [SerializeField] private Animator tutorialPanelAnimator;

    private void Start()
    {
        tutorialPanel.SetActive(false);
        TutorialManager.Instance.OnStepStart += TutorialManager_OnStepStart;
        TutorialManager.Instance.OnStepEnd += TutorialManager_OnStepEnd;
    }

    private void TutorialManager_OnStepStart(object sender, TutorialStepStartEvent e)
    {
        Show(e.TutorialName, e.TutorialText);
    }
    
    private void TutorialManager_OnStepEnd(object sender, EventArgs e)
    {
        Hide();
    }

    private void Show(string tutorialName, string tutorialDesc)
    {
        StopCoroutine(HidePanel());
        tutorialPanelAnimator.SetTrigger("start");
        tutorialPanel.SetActive(true);
        tutorialNameText.text = tutorialName;
        tutorialDescText.text = tutorialDesc;
    }
    
    private void Hide()
    {
        StartCoroutine(HidePanel());
    }
    
    private IEnumerator HidePanel()
    {
        tutorialPanelAnimator.SetTrigger("end");
        yield return new WaitForSeconds(1f);
        tutorialPanel.SetActive(false);
    }
}