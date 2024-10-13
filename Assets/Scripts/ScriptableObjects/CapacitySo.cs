using UnityEngine;

[CreateAssetMenu(fileName = "Capacity_new", menuName = "ScriptableObject/Capacity", order = 0)]
public class CapacitySo : ScriptableObject
{
    public string capacityName;
    public Sprite icon;
    public int price;
}