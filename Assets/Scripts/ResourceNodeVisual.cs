using System;
using UnityEngine;

public class ResourceNodeVisual : MonoBehaviour
{
    private static readonly int Harvest = Animator.StringToHash("Harvest");
    
    [SerializeField] private Animator animator;
    [SerializeField] private ResourceNode resourceNode;

    private void Start()
    {
        resourceNode.OnStartHarvest += ResourceNode_OnStartHarvest;
    }
    
    private void ResourceNode_OnStartHarvest(object sender, EventArgs e)
    {
        animator.SetTrigger(Harvest);
    }
}
