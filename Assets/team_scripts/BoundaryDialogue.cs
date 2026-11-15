using UnityEngine;
using TMPro;

public class BoundaryDialogue : MonoBehaviour
{
    public GameObject dialogueBox;
    public TextMeshProUGUI dialogueText; 
    public float displayDuration = 3f;

    private bool isDisplaying;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (dialogueBox != null)
            {
                if (dialogueText != null){
                dialogueBox.SetActive(true);
                isDisplaying = true;
                }
            }
        }
    }

    private void Update()
    {
        if (isDisplaying && Input.GetKeyDown(KeyCode.E))
            {
                dialogueBox.SetActive(false);
                isDisplaying = false;
            }
    }
}