using System;
using UnityEngine;

public class WoodcuttingStationVisual : MonoBehaviour
{
    private const string Processing = "Processing";
    [SerializeField] private WoodcuttingStation woodcuttingStation;
    [SerializeField] private Transform productSlot;
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    private void Start() {
        woodcuttingStation.OnProcessing += WoodcuttingStation_OnProcessing;
        woodcuttingStation.OnStopProcessing += WoodcuttingStation_OnStopProcessing;
    }

    private void WoodcuttingStation_OnStopProcessing(object sender, EventArgs e)
    {
        animator.SetBool(Processing, false);
    }

    private void WoodcuttingStation_OnProcessing(object sender, EventArgs e)
    {
        animator.SetBool(Processing, true);
    }
}
