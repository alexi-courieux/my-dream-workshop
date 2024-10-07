using UnityEngine;

namespace AshLight.BakerySim.ScriptableObjects
{
    [CreateAssetMenu(fileName = "NewResourceNode", menuName = "ScriptableObject/ResourceNode", order = 0)]
    public class ResourceNodeSo : ScriptableObject
    {
        public ProductSo product;
        public int maxResource;
        public float timeToRegen;
        public int interactionCountToHarvest;
        public float timeBetweenInteractions;
    }
}