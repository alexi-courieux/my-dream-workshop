using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    private static readonly int EndAnimation = Animator.StringToHash("end");
    private static readonly int StartAnimation = Animator.StringToHash("start");
    
    [SerializeField] private TMP_Text tutorialNameText;
    [SerializeField] private TMP_Text tutorialDescText;
    [SerializeField] private GameObject tutorialPanel;
    
    [SerializeField] private RectTransform movingPointer;
    [SerializeField] private float pointerOffset = 2;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float pointerLerpDuration = 10f;
    [SerializeField] private float playerNearDistance = 1f;
    [SerializeField] private Transform staticPointerPrefab;
    [SerializeField] private float staticPointerHeightOffset = 3.5f;
    
    private bool isPointerActive;
    private bool isPlayerNearTarget;
    private Transform targetTransform;
    private Transform staticPointerInstance;
    private Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main;
        staticPointerInstance = Instantiate(staticPointerPrefab, Vector3.zero, Quaternion.identity, null);
    }

    private void Start()
    {
        tutorialPanel.SetActive(false);
        TutorialManager.Instance.OnStepStart += TutorialManager_OnStepStart;
        TutorialManager.Instance.OnStepEnd += TutorialManager_OnStepEnd;
        HidePointer();
    }

    private void Update()
    {
        if (!isPointerActive) return;
        CheckPlayerDistanceFromTarget();
        MovePointer();
    }

    private void CheckPlayerDistanceFromTarget()
    {
        float distance = Vector3.Distance(targetTransform.position, playerTransform.position);
        if (distance < playerNearDistance)
        {
            if (isPlayerNearTarget) return;
            isPlayerNearTarget = true;
            movingPointer.gameObject.SetActive(false);
        }
        else
        {
            if (!isPlayerNearTarget) return;
            isPlayerNearTarget = false;
            movingPointer.gameObject.SetActive(true);
        }
    }
    
    private void MovePointer()
    {
        if (isPlayerNearTarget) return;
        
        Vector3 direction = targetTransform.position - playerTransform.position;
        float distance = direction.magnitude;
        float clampedDistance = Mathf.Min(distance, pointerOffset);
        
        Vector3 clampedPosition = playerTransform.position + direction.normalized * clampedDistance;
        Vector3 targetScreenPosition = mainCamera.WorldToScreenPoint(clampedPosition);

        movingPointer.position = Vector3.Lerp(movingPointer.position, targetScreenPosition, Time.deltaTime * pointerLerpDuration);

        float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
        angle = (angle + 180) % 360;
        movingPointer.rotation = Quaternion.Euler(0, 0, angle);
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
        isPlayerNearTarget = true; // to force update
        staticPointerInstance.gameObject.SetActive(true);
        staticPointerInstance.position = targetTransform.position + Vector3.up * staticPointerHeightOffset;
    }
    
    private void Hide()
    {
        HidePointer();
        tutorialPanel.SetActive(false);
    }
    
    private void HidePointer()
    {
        isPointerActive = false;
        movingPointer.gameObject.SetActive(false);
        staticPointerInstance.gameObject.SetActive(false);
    }
}