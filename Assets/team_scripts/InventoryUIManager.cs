using UnityEngine;
using TMPro;

public class InventoryUIManager : MonoBehaviour
{
    public static InventoryUIManager Instance;

    [Header("UI References")]
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text descriptionText;

    [Header("Preview Window Reference")]
    [SerializeField] private Animator previewAnimator;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        ClearHoverInfo();
        ClearPreview();
    }

    // Called on HOVER
    public void DisplayHoverInfo(ItemData itemData)
    {
        if (itemData == null) return;
        if (nameText != null) nameText.text = itemData.itemName;
        if (descriptionText != null) descriptionText.text = itemData.itemDescription;
    }

    // Called when leaving slot
    public void ClearHoverInfo()
    {
        if (nameText != null) nameText.text = "";
        if (descriptionText != null) descriptionText.text = "Hover over an item to inspect.";
    }

    // Called on CLICK or PICKUP to play the animation controller
    public void SelectAndAnimateItem(ItemData itemData)
    {
        if (previewAnimator == null || itemData == null) return;

        if (itemData.itemAnimatorController != null)
        {
            previewAnimator.gameObject.SetActive(true);
            previewAnimator.runtimeAnimatorController = itemData.itemAnimatorController;
        }
        else
        {
            ClearPreview();
        }
    }

    public void ClearPreview()
    {
        if (previewAnimator == null) return;
        previewAnimator.runtimeAnimatorController = null;
        previewAnimator.gameObject.SetActive(false);
    }
}