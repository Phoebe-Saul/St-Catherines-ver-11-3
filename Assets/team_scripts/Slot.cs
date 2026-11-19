using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public GameObject currentItem;
    [HideInInspector] public int slotIndex;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentItem != null)
        {
            ItemData data = currentItem.GetComponent<ItemData>();
            if (data != null)
            {
                InventoryUIManager.Instance?.DisplayHoverInfo(data);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryUIManager.Instance?.ClearHoverInfo();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentItem != null)
        {
            ItemData data = currentItem.GetComponent<ItemData>();
            if (data != null)
            {
                InventoryUIManager.Instance?.SelectAndAnimateItem(data);
                InventoryController.Instance?.SelectSlot(slotIndex);
            }
        }
    }
}