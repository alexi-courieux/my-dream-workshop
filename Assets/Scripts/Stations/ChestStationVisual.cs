using System;
using UnityEngine;

public class ChestStationVisual : MonoBehaviour
{
    private static readonly int Open = Animator.StringToHash("Open");
    
    [SerializeField] ChestStation chestStation;
    [SerializeField] private Animator animator;

    private void Start()
    {
        chestStation.OnProductAmountChanged += ChestStation_OnProductAmountChanged;
    }
    
    private void ChestStation_OnProductAmountChanged(object sender, EventArgs e)
    {
        animator.SetTrigger(Open);
    }
}
