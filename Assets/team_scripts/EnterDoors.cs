using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterDoors : MonoBehaviour
{
    [SerializeField] private string targetSceneName;
    [SerializeField] private string targetSpawnId;
    [SerializeField] private bool requireInteractKey = true;
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    private bool playerInRange;

    private void Update()
    {
        if (requireInteractKey && playerInRange && Input.GetKeyDown(interactKey))
        {
            LoadTargetScene();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInRange = true;
        if (!requireInteractKey)
        {
            LoadTargetScene();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    private void LoadTargetScene()
    {
        if (string.IsNullOrEmpty(targetSceneName))
        {
            Debug.LogWarning("Target Scene Name is empty on " + gameObject.name);
            return;
        }

        DoorSpawnContext.NextSpawnId = targetSpawnId;
        SceneManager.LoadScene(targetSceneName);
    }
}