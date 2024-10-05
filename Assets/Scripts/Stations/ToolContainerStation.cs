using UnityEngine;

public class ToolContainerStation : MonoBehaviour, IInteractable
{
    [SerializeField] private ToolSo toolSo;

    public void Interact()
    {
        if (Player.Instance.HandleSystem.HaveAnyItems()) return;
        Item.SpawnItem<Tool>(toolSo.prefab, Player.Instance.HandleSystem);
    }
}