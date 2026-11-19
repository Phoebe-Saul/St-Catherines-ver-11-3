using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryController : MonoBehaviour
{
    public static InventoryController Instance;

    [Header("Grid & Slot Setup")]
    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int slotCount = 12;
    public GameObject[] itemPrefabs; // Optional starting items

    [Header("World Drop Settings")]
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float dropDistance = 1.2f;

    private readonly List<Slot> spawnedSlots = new List<Slot>();
    private readonly List<GameObject> slotItemPrefabs = new List<GameObject>();
    private readonly List<int> slotQuantities = new List<int>();

    private int selectedSlotIndex = -1;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        for (int i = 0; i < slotCount; i++)
        {
            Slot slot = Instantiate(slotPrefab, inventoryPanel.transform).GetComponent<Slot>();
            slot.slotIndex = i;

            spawnedSlots.Add(slot);
            slotItemPrefabs.Add(null);
            slotQuantities.Add(0);

            if (itemPrefabs != null && i < itemPrefabs.Length && itemPrefabs[i] != null)
            {
                PlacePrefabInSlot(i, itemPrefabs[i], 1);
            }
        }
    }

    // Called by WorldPickup when pressing E
    public bool AddItem(GameObject itemPrefabToAdd, int amount = 1)
    {
        if (itemPrefabToAdd == null || amount <= 0) return false;

        ItemData incomingData = itemPrefabToAdd.GetComponent<ItemData>();
        int remaining = amount;

        // 1. Stack into matching slots if stackable
        if (incomingData != null && incomingData.isStackable)
        {
            for (int i = 0; i < spawnedSlots.Count; i++)
            {
                if (spawnedSlots[i].currentItem == null) continue;

                ItemData existingData = spawnedSlots[i].currentItem.GetComponent<ItemData>();
                if (existingData != null && existingData.itemName == incomingData.itemName && slotQuantities[i] < incomingData.maxStackSize)
                {
                    int space = incomingData.maxStackSize - slotQuantities[i];
                    int toMove = Mathf.Min(space, remaining);
                    slotQuantities[i] += toMove;
                    remaining -= toMove;
                    UpdateSlotQuantityText(i);

                    if (remaining <= 0) return true;
                }
            }
        }

        // 2. Place remaining items into open slots
        for (int i = 0; i < spawnedSlots.Count; i++)
        {
            if (spawnedSlots[i].currentItem == null)
            {
                int cap = (incomingData != null && incomingData.isStackable) ? incomingData.maxStackSize : 1;
                int toPlace = Mathf.Min(cap, remaining);

                PlacePrefabInSlot(i, itemPrefabToAdd, toPlace);
                remaining -= toPlace;

                if (remaining <= 0) return true;
            }
        }

        if (remaining == amount)
        {
            Debug.Log("Inventory is full!");
            return false;
        }

        return true;
    }

    private void PlacePrefabInSlot(int index, GameObject itemPrefab, int quantity)
    {
        GameObject item = Instantiate(itemPrefab, spawnedSlots[index].transform);
        item.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

        spawnedSlots[index].currentItem = item;
        slotItemPrefabs[index] = itemPrefab;
        slotQuantities[index] = quantity;

        UpdateSlotQuantityText(index);
    }

    public void SelectSlot(int index)
    {
        if (index < 0 || index >= spawnedSlots.Count || spawnedSlots[index].currentItem == null)
        {
            selectedSlotIndex = -1;
            return;
        }

        selectedSlotIndex = index;
    }

    // Hook to your "Use" UI button
    public void UseSelectedItem()
    {
        if (selectedSlotIndex < 0 || selectedSlotIndex >= spawnedSlots.Count) return;
        GameObject current = spawnedSlots[selectedSlotIndex].currentItem;
        if (current == null) return;

        ItemData data = current.GetComponent<ItemData>();
        if (data != null)
        {
            Debug.Log($"Used {data.itemName}");
        }

        ConsumeOneFromSelectedSlot();
    }

    // Hook to your "Drop" UI button
    public void DropSelectedItem()
    {
        if (selectedSlotIndex < 0 || selectedSlotIndex >= spawnedSlots.Count) return;
        GameObject current = spawnedSlots[selectedSlotIndex].currentItem;
        if (current == null) return;

        ItemData data = current.GetComponent<ItemData>();
        if (data != null && data.worldPrefab != null && playerTransform != null)
        {
            Vector3 dropPos = playerTransform.position + (Vector3)(playerTransform.up * dropDistance);
            GameObject dropped = Instantiate(data.worldPrefab, dropPos, Quaternion.identity);
            if (dropped.TryGetComponent(out WorldPickup pickup))
            {
                pickup.SetItem(slotItemPrefabs[selectedSlotIndex], 1);
            }
        }

        ConsumeOneFromSelectedSlot();
    }

    private void ConsumeOneFromSelectedSlot()
    {
        slotQuantities[selectedSlotIndex]--;
        if (slotQuantities[selectedSlotIndex] <= 0)
        {
            Destroy(spawnedSlots[selectedSlotIndex].currentItem);
            spawnedSlots[selectedSlotIndex].currentItem = null;
            slotItemPrefabs[selectedSlotIndex] = null;
            slotQuantities[selectedSlotIndex] = 0;

            selectedSlotIndex = -1;
            InventoryUIManager.Instance?.ClearHoverInfo();
            InventoryUIManager.Instance?.ClearPreview();
        }
        else
        {
            UpdateSlotQuantityText(selectedSlotIndex);
        }
    }

    private void UpdateSlotQuantityText(int index)
    {
        GameObject itemObj = spawnedSlots[index].currentItem;
        if (itemObj == null) return;

        TMP_Text qtyText = itemObj.GetComponentInChildren<TMP_Text>();
        if (qtyText != null)
        {
            bool showCount = slotQuantities[index] > 1;
            qtyText.text = showCount ? slotQuantities[index].ToString() : "";
            qtyText.gameObject.SetActive(showCount);
        }
    }
}