using System;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Utils;

public class PlayerItemHandlingSystem : MonoBehaviour, IHandleItems
{
    public event EventHandler OnSlotSelected;
    
    private const int BackpackSlots = 7;
    private const int DefaultSlots = 3;
    private const int MaxAmountPerSlot = 10;
    
    [SerializeField] private Transform itemSlot;
    [SerializeField] private GameObject backpackVisual;
    [SerializeField] private ProductDictionarySo backpackProductDictionarySo;

    internal Item[,] items;
    private ItemInventory itemInventory;
    private int selectedSlotIndex;
    
    public int SelectedSlotIndex => selectedSlotIndex;

    private void Awake()
    {
        itemInventory = new ItemInventory(DefaultSlots, MaxAmountPerSlot);
        items = new Item[DefaultSlots, MaxAmountPerSlot];
        selectedSlotIndex = 0;
    }

    private void Start()
    {
        backpackVisual.SetActive(false);
        InputManager.Instance.OnNextSlot += InputManager_OnNextSlot;
        InputManager.Instance.OnPreviousSlot += InputManager_OnPreviousSlot;
    }
    
    private void InputManager_OnNextSlot(object sender, EventArgs e)
    {
        selectedSlotIndex++;
        if (selectedSlotIndex >= items.GetLength(0)) selectedSlotIndex = 0;
        RefreshItemVisuals();
        OnSlotSelected?.Invoke(this, EventArgs.Empty);
    }
    
    private void InputManager_OnPreviousSlot(object sender, EventArgs e)
    {
        selectedSlotIndex--;
        if (selectedSlotIndex < 0) selectedSlotIndex = items.GetLength(0) - 1;
        RefreshItemVisuals();
        OnSlotSelected?.Invoke(this, EventArgs.Empty);
    }

    public void AddItem(Item newItem)
    {
        if (newItem is not Product product)
        {
            // TODO Player must be able to handle items, not only products
            Debug.LogError("Trying to add an item that is not a product");
            return;
        }
        if (!itemInventory.CanAddItem(product.ProductSo)) throw new Exception("Trying to add an item but no available slots");

        int[] itemSlotIndexes = itemInventory.GetItemSlots(product.ProductSo);
        if (itemSlotIndexes.Length > 0)
        {
            foreach (int itemSlotIndex in itemSlotIndexes)
            {
                var slot = itemInventory.GetSlot(itemSlotIndex);
                if (slot.Amount + 1 <= MaxAmountPerSlot)
                {
                    // Add item to existing slot
                    itemInventory.AddItem(itemSlotIndex, product.ProductSo, 1);
                    items[itemSlotIndex, slot.Amount - 1] = product;
                    RefreshItemVisuals();
                    return;
                }
            }
        }
        
        var selectedSlot = itemInventory.GetSlot(selectedSlotIndex);
        if (selectedSlot is null)
        {
            // Add item to selected slot
            itemInventory.AddItem(selectedSlotIndex, product.ProductSo, 1);
            items[selectedSlotIndex, 0] = product;
            RefreshItemVisuals();
            return;
        }
        
        // Add item to first available slot
        int index = itemInventory.TryAddItem(product.ProductSo, 1);
        items[index, 0] = product;
        RefreshItemVisuals();
    }

    public Item[] GetItems<T>() where T : Item
    {
        return FlattenItems().Where(item => item is T).ToArray();
    }

    public Item GetSelectedItem()
    {
        int slotAmount = itemInventory.GetSlot(selectedSlotIndex).Amount;
        return items[selectedSlotIndex, slotAmount - 1];
    }
    
    public T GetItem<T>() where T : Item
    {
        int slotAmount = itemInventory.GetSlot(selectedSlotIndex).Amount;
        return items[selectedSlotIndex, slotAmount - 1] as T;
    }

    public void ClearItem(Item itemToClear)
    {
        Product product = itemToClear as Product;
        items[selectedSlotIndex, itemInventory.GetSlot(selectedSlotIndex).Amount - 1] = null;
        itemInventory.RemoveItem(product!.ProductSo, 1, selectedSlotIndex);
        RefreshItemVisuals();
    }

    public bool HaveItems<T>() where T : Item
    {
        return FlattenItems().Any(item => item is T);
    }
    
    public bool HaveItemSelected<T>() where T : Item
    {
        return items[selectedSlotIndex, 0] is T;
    }

    public bool HaveAnyItems()
    {
        return itemInventory.GetItems().Count > 0;
    }
    
    public bool HaveAnyItemSelected()
    {
        return items[selectedSlotIndex, 0] != null;
    }

    public bool HaveSpace(ItemSo itemToAdd)
    {
        return itemInventory.CanAddItem(itemToAdd);
    }

    public Transform GetAvailableItemSlot(Item newItem)
    {
        return itemSlot;
    }

    public bool HasAvailableSlot(Item newItem)
    {
        if (newItem is not Product product) return false;
        return itemInventory.CanAddItem(product.ProductSo);
    }
    
    public void EquipBackpack()
    {
        backpackVisual.SetActive(true);
        itemInventory.AddSlots(BackpackSlots);
        ArrayUtils.Resize2DArray(items, items.GetLength(0) + BackpackSlots, MaxAmountPerSlot);
    }
    
    public bool UnequipBackpack()
    {
        if (!itemInventory.TryRemoveSlots(BackpackSlots)) return false;
        backpackVisual.SetActive(false);
        for (int i =0; i < itemInventory.GetSlotAmount(); i++)
        {
            ItemSo itemSo = itemInventory.GetSlot(i)?.Item;
            if (itemSo is null) continue;
            bool found = false;
            for (int j = i; j < items.GetLength(0) && !found; j++)
            {
                for (int k = 0; k < MaxAmountPerSlot; k++)
                {
                    if (items[j, k] is null) continue;
                    if (items[j, k] is not Product product) continue;
                    if (!product.ProductSo.Equals(itemSo)) continue;
                    {
                        items[i, k] = product;
                        items[j, k] = null;
                        found = true;
                        break;
                    }
                }
            }
        }
        ArrayUtils.Resize2DArray(items, items.GetLength(0) - BackpackSlots, MaxAmountPerSlot);
        selectedSlotIndex = Mathf.Min(selectedSlotIndex, items.GetLength(0) - 1);
        RefreshItemVisuals();
        OnSlotSelected?.Invoke(this, EventArgs.Empty);
        return true;
    }
    
    public ItemInventory GetProductInventory()
    {
        return itemInventory;
    }
    
    private void RefreshItemVisuals()
    {
        for (int i = 0; i < items.GetLength(0); i++)
        {
            int slotAmount = itemInventory.GetSlot(i)?.Amount ?? 0;
            for (int j = 0; j < slotAmount; j++)
            {
                bool isActive = i == selectedSlotIndex && j == slotAmount - 1;
                items[i, j]?.gameObject.SetActive(isActive);
            }
        }
    }
    
    private Item[] FlattenItems()
    {
        return items.Cast<Item>().Where(item => item is not null).ToArray();
    }
}

[CustomEditor(typeof(PlayerItemHandlingSystem))]
public class PlayerItemHandlingSystemEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        PlayerItemHandlingSystem system = (PlayerItemHandlingSystem)target;

        if (system.items != null)
        {
            for (int i = 0; i < system.items.GetLength(0); i++)
            {
                EditorGUILayout.LabelField($"Row {i}");
                EditorGUI.indentLevel++;
                for (int j = 0; j < system.items.GetLength(1); j++)
                {
                    system.items[i, j] = (Item)EditorGUILayout.ObjectField($"Item {i},{j}", system.items[i, j], typeof(Item), true);
                }
                EditorGUI.indentLevel--;
            }
        }
    }
}