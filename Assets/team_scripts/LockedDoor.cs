
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LockedDoor : MonoBehaviour
{
    //[SerializeField] private string nextSceneName;
    [SerializeField] private GameObject closedGate, openGate;
    [SerializeField] private SpriteRenderer interactPrompt;
    private bool playerInRange;

    void Start()
    {
        if (interactPrompt != null)
            interactPrompt.enabled = false;
    }

    void Update()
    {
        if (!playerInRange)
            return;

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (KeyManager.Instance.CollectedKeys >= 6)
            {
                Debug.Log("All keys collected! Door unlocked!");
                closedGate.SetActive(false);
                openGate.SetActive(true);
                //SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                Debug.Log("You need all six keys!");
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            if (interactPrompt != null)
                interactPrompt.enabled = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            if (interactPrompt != null)
                interactPrompt.enabled = false;
        }
    }
}
