using UnityEngine;

public class ItemData : MonoBehaviour
{
    [Header("Item Info")]
    public string itemName;
    [TextArea(3, 10)]
    public string itemDescription;

    [Header("Preview Animation Sprites")]
    public RuntimeAnimatorController itemAnimatorController;

    [Header("Stacking & World Drop")]
    public bool isStackable = false;
    public int maxStackSize = 1;
    public GameObject worldPrefab;
}