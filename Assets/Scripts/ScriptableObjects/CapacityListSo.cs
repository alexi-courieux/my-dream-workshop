using UnityEngine;

[CreateAssetMenu(fileName = "capacityList", menuName = "ScriptableObject/CapacityList", order = 0)]
public class CapacityListSo : ScriptableObject
{
    public CapacitySo[] capacities;
}
