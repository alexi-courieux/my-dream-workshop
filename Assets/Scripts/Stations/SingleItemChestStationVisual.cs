using System;
using UnityEngine;
using UnityEngine.Serialization;

public class SingleItemChestStationVisual : MonoBehaviour
{
    private static readonly int Open = Animator.StringToHash("Open");
    
    [FormerlySerializedAs("singleChestStation")] [FormerlySerializedAs("chestStation")] [SerializeField] SingleItemChestStation singleItemChestStation;
    [SerializeField] private Animator animator;

    private void Start()
    {
        singleItemChestStation.OnProductAmountChanged += SingleItemChestStationOnProductAmountChanged;
    }
    
    private void SingleItemChestStationOnProductAmountChanged(object sender, EventArgs e)
    {
        animator.SetTrigger(Open);
    }
}
