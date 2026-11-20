
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactable : MonoBehaviour
{
    public enum InteractionType
    {
        Dialogue,
        NumberLock
    }

    [Header("Interaction")]
    [SerializeField] private InteractionType interactionType;

    [SerializeField] private DialogueTrigger dialogueTrigger;
    [SerializeField] private NumberLock numberLock;

    [SerializeField] private GameObject interactionPrompt;

    private bool playerInRange;

    
    void Update()
    {
        if (Keyboard.current == null)
            return;

        if (!playerInRange)
            return;

        if (DialogueManager.Instance != null)
        {
            if (DialogueManager.Instance.IsDialogueActive)
                return;

            if (interactionType == InteractionType.Dialogue &&
                DialogueManager.Instance.InteractionCooldown)
                return;
        }

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            Interact();
        }
    }


    void Interact()
    {
        Debug.Log("INTERACT METHOD WAS CALLED");
        switch (interactionType)
        {
            case InteractionType.Dialogue:
                if (dialogueTrigger != null)
                    dialogueTrigger.TriggerDialogue();
                break;

            case InteractionType.NumberLock:
                if (numberLock != null)
                    numberLock.OpenLock();
                break;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInRange = true;

        if (interactionPrompt != null)
            interactionPrompt.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInRange = false;

        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
    }
}
