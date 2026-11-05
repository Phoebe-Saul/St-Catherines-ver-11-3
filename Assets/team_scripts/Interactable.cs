
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactable : MonoBehaviour
{
    [Header("Interaction")]
    [SerializeField] private DialogueTrigger dialogueTrigger;
    [SerializeField] private GameObject interactionPrompt;

    private bool playerInRange;

    void Update()
    {
        if (DialogueManager.Instance == null)
            return;

        if (DialogueManager.Instance.IsDialogueActive || DialogueManager.Instance.InteractionCooldown)
            return;

        if (playerInRange && Keyboard.current != null &&
            Keyboard.current.eKey.wasPressedThisFrame)
        {
            Interact();
        }
    }

    void Interact()
    {
        if (dialogueTrigger != null)
        {
            dialogueTrigger.TriggerDialogue();
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
