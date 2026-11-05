
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text speakerNameText;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Image portraitImage;
    private bool interactionCooldown = false;
    public bool InteractionCooldown => interactionCooldown;

    private DialogueData currentDialogue;
    private int currentLine;

    public bool IsDialogueActive { get; private set; }

    private bool waitForERelease = false;

    void Awake()
    {
        Instance = this;
        dialoguePanel.SetActive(false);
    }

    void Update()
    {
        if (!IsDialogueActive)
            return;

        // Wait until E is released after starting dialogue
        if (waitForERelease)
        {
            if (Keyboard.current.eKey.wasReleasedThisFrame)
            {
                waitForERelease = false;
            }

            return;
        }

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            NextLine();
        }
    }

    public void StartDialogue(DialogueData dialogue)
    {
        currentDialogue = dialogue;
        currentLine = 0;

        IsDialogueActive = true;
        waitForERelease = true;

        dialoguePanel.SetActive(true);

        speakerNameText.text = dialogue.characterName;
        portraitImage.sprite = dialogue.portrait;

        ShowLine();
    }

    void ShowLine()
    {
        dialogueText.text = currentDialogue.dialogueLines[currentLine];
    }

    void NextLine()
    {
        currentLine++;

        if (currentLine < currentDialogue.dialogueLines.Length)
        {
            ShowLine();
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        IsDialogueActive = false;

        StartCoroutine(DialogueCooldown());
    }

    IEnumerator DialogueCooldown()
    {
        interactionCooldown = true;

        yield return new WaitForSeconds(1f);

        interactionCooldown = false;
    }
}
