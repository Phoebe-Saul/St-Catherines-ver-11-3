using UnityEngine;

public class ItemData : MonoBehaviour
{
    [Header("Item Info")]
    public string itemName;
    [TextArea(3, 10)]
    public string itemDescription;

    [Header("Preview Animation Sprites")]
    public RuntimeAnimatorController itemAnimatorController;

}
