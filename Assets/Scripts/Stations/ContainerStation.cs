using UnityEngine;
public class ContainerStation : MonoBehaviour, IInteractable
{
    [SerializeField] private ProductSo productSo;
    [SerializeField] private SpriteRenderer containerSprite;
    private void Start()
    {
        containerSprite.sprite = productSo.sprite;
    }

    public void Interact()
    {
        if (!Player.Instance.HandleSystem.HaveSpace(productSo)) return;
        Item.SpawnItem(productSo.prefab, Player.Instance.HandleSystem);
    }
}