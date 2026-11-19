using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class WorldPickup : MonoBehaviour
{
    [Header("Item Settings")]
    [Tooltip("Drag the UI item prefab (with ItemData attached) that goes into the inventory slot")]
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private int quantity = 1;
    [SerializeField] private bool pickupOnTouch = false;

    private bool playerInRange;

    private void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void Update()
    {
        if (!pickupOnTouch && playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            TryCollect();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (pickupOnTouch)
        {
            TryCollect();
        }
        else
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void TryCollect()
    {
        if (itemPrefab == null)
        {
            Debug.LogWarning($"WorldPickup on {gameObject.name} is missing an itemPrefab reference!");
            return;
        }

        // 1. Add the item prefab to an open/stackable slot in InventoryController
        if (InventoryController.Instance != null && !InventoryController.Instance.AddItem(itemPrefab, quantity))
        {
            return; // Inventory is full—leave item on the ground
        }

        // 2. Preview/select the item in InventoryUIManager
        if (InventoryUIManager.Instance != null && itemPrefab.TryGetComponent(out ItemData data))
        {
            InventoryUIManager.Instance.DisplayHoverInfo(data);
            InventoryUIManager.Instance.SelectAndAnimateItem(data);
        }

        // 3. Remove the pickup from the 2D map
        Destroy(gameObject);
    }

    public void SetItem(GameObject newItemPrefab, int newQuantity = 1)
    {
        itemPrefab = newItemPrefab;
        quantity = Mathf.Max(1, newQuantity);
    }
}