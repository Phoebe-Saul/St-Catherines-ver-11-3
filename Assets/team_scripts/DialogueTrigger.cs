
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private DialogueData dialogueData;

    public void TriggerDialogue()
    {
        if (dialogueData != null)
        {
            DialogueManager.Instance.StartDialogue(dialogueData);
        }
    }
}
