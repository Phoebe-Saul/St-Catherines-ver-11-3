using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public GameObject currentItem;

    // Triggered when mouse enters the slot
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentItem != null)
        {
            ItemData data = currentItem.GetComponent<ItemData>();
            if (data != null)
            {
                InventoryUIManager.Instance.DisplayHoverInfo(data);
            }
        }
    }

    // Triggered when mouse leaves the slot
    public void OnPointerExit(PointerEventData eventData)
    {
        InventoryUIManager.Instance.ClearHoverInfo();
    }

    // Triggered when you CLICK on the slot/item
    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentItem != null)
        {
            ItemData data = currentItem.GetComponent<ItemData>();
            if (data != null)
            {
                InventoryUIManager.Instance.SelectAndAnimateItem(data);
            }
        }
    }
}