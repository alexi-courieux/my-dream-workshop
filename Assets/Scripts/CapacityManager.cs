using System;
using System.Collections.Generic;
using UnityEngine;

public class CapacityManager : MonoBehaviour
{
    public static CapacityManager Instance { get; private set; }

    private List<CapacitySo> capacities;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        capacities = new List<CapacitySo>();
    }
    
    public void AddCapacity(CapacitySo capacity)
    {
        capacities.Add(capacity);
    }
    
    public CapacitySo[] GetCapacities()
    {
        return capacities.ToArray();
    }
}
