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
    
    [SerializeField] private RectTransform pointer;
    [SerializeField] private float pointerOffset = 2;
    [SerializeField] private Transform playerPosition;
    [SerializeField] private float pointerLerpDuration = 10f;
    [SerializeField] private float pointerHideDistance = 1f;
    
    private bool isPointerActive;
    private Transform targetTransform;

    private void Start()
    {
        tutorialPanel.SetActive(false);
        TutorialManager.Instance.OnStepStart += TutorialManager_OnStepStart;
        TutorialManager.Instance.OnStepEnd += TutorialManager_OnStepEnd;
        isPointerActive = false;
    }

    private void Update()
    {
        if (pointer.gameObject.activeSelf)
        {
            MovePointer();
        }
    }
    
    private void MovePointer()
    {
        if (!isPointerActive) return;
        
        Vector3 direction = targetTransform.position - playerPosition.position;
        float distance = direction.magnitude;
        float clampedDistance = Mathf.Min(distance, pointerOffset);
        
        Vector3 clampedPosition = playerPosition.position + direction.normalized * clampedDistance;
        Vector3 targetScreenPosition = Camera.main.WorldToScreenPoint(clampedPosition);

        pointer.position = Vector3.Lerp(pointer.position, targetScreenPosition, Time.deltaTime * pointerLerpDuration);

        float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
        angle = (angle + 180) % 360;
        pointer.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void TutorialManager_OnStepStart(object sender, TutorialStepStartEvent e)
    {
        Show(e);
    }
    
    private void TutorialManager_OnStepEnd(object sender, EventArgs e)
    {
        Hide();
    }

    private void Show(TutorialStepStartEvent e)
    {
        StopCoroutine(HidePanel());
        tutorialPanelAnimator.SetTrigger("start");
        tutorialPanel.SetActive(true);
        tutorialNameText.text = e.TutorialName;
        tutorialDescText.text = e.TutorialText;
        if (e.Objective is not null)
        {
            ShowPointer(e.Objective);
        }
    }
    
    private void ShowPointer(Transform target)
    {
        targetTransform = target;
        isPointerActive = true;
        pointer.gameObject.SetActive(true);
        MovePointer();
    }
    
    private void Hide()
    {
        HidePointer();
        StartCoroutine(HidePanel());
    }
    
    private void HidePointer()
    {
        isPointerActive = false;
        pointer.gameObject.SetActive(false);
    }
    
    private IEnumerator HidePanel()
    {
        tutorialPanelAnimator.SetTrigger("end");
        yield return new WaitForSeconds(1f);
        tutorialPanel.SetActive(false);
    }
}