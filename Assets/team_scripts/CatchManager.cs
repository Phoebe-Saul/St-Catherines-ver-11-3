
using UnityEngine;

public class CatchManager : MonoBehaviour
{
    public static CatchManager Instance;

    [Header("Player")]
    [SerializeField] private Transform player;
    [SerializeField] private Transform respawnPoint;

    [Header("Caught Dialogue")]
    [SerializeField] private DialogueData normalCaughtDialogue;
    [SerializeField] private DialogueData thirdCaughtDialogue;

    private int catchCount = 0;

    public int CatchCount => catchCount;

    void Awake()
    {
        Instance = this;
    }

    public void PlayerCaught()
    {
        catchCount++;

        Debug.Log("Mary has been caught " + catchCount + " times!");

        // Choose dialogue
        if (catchCount >= 3)
        {
            KeyManager.Instance.ResetKeys();
            DialogueManager.Instance.StartDialogue(thirdCaughtDialogue);
            catchCount = 0;
        }
        else
        {
            DialogueManager.Instance.StartDialogue(normalCaughtDialogue);
        }

        // Respawn Mary
        player.position = respawnPoint.position;
    }
}
