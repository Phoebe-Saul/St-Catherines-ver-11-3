using UnityEngine;

public class InteractableSavePoint : MonoBehaviour
{
    [Header("Interaction Settings")]
    [SerializeField] private float interactRange = 2f;

    [Header("UI Reference")]
    [SerializeField] private GameObject savePromptUI;

    private Transform playerTransform;
    private SaveController saveController;

    void Start(){
        saveController = FindFirstObjectByType<SaveController>();
        if(saveController == null){
            Debug.LogError("SaveController not found in scene.");
        }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player != null){
            playerTransform = player.transform;
            Debug.Log("Player next to shrine.");
        }
        else{
            Debug.LogError("Player not found.");
        }

        if(savePromptUI != null){
            savePromptUI.SetActive(false);
        }
    }

    void Update(){
        if(playerTransform == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        bool canInteract = (distanceToPlayer <= interactRange && SafeZone.IsPlayerSafe);
        //turn on/off prompt 
        if(savePromptUI != null){
            savePromptUI.SetActive(canInteract);
        }

        //save by pressing e when near
        if(canInteract && Input.GetKeyDown(KeyCode.E)){
            TriggerSave();
        }

    }

    void TriggerSave(){
        if(saveController != null){
            saveController.SaveGame();
            Debug.Log("Game saved at shrine (InteractableSavePoint.cs)");
        }
    }

    //to see zone in editor 
    void OnDrawGizmosSelected(){
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
