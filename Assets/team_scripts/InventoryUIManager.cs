using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryUIManager : MonoBehaviour
{
    public static InventoryUIManager Instance;

    [Header("UI References")]
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;
    
    [Header("Preview Window Reference")]
    [SerializeField] private Animator previewAnimator; // <--- This was missing from your class variables

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        ClearHoverInfo();
    }

    // Called on HOVER
    public void DisplayHoverInfo(ItemData itemData)
    {
        if (nameText != null) nameText.text = itemData.itemName;
        if (descriptionText != null) descriptionText.text = itemData.itemDescription;
    }

    // Called when leaving slot
    public void ClearHoverInfo()
    {
        if (nameText != null) nameText.text = "";
        if (descriptionText != null) descriptionText.text = "Hover over an item to inspect.";
    }

    // Called on CLICK to play the animation controller
    public void SelectAndAnimateItem(ItemData itemData)
    {
        if (previewAnimator == null) return;

        if (itemData.itemAnimatorController != null)
        {
            previewAnimator.gameObject.SetActive(true);
            previewAnimator.runtimeAnimatorController = itemData.itemAnimatorController;
        }
        else
        {
            previewAnimator.gameObject.SetActive(false);
        }
    }
}