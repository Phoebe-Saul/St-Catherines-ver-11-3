
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnOutside : MonoBehaviour
{
    [SerializeField] private string spawnId;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player == null) return;

        // Returning from a house
        if (DoorSpawnContext.HasReturnPosition &&
            SceneManager.GetActiveScene().name ==
            DoorSpawnContext.ReturnSceneName)
        {
            player.transform.position = DoorSpawnContext.ReturnPosition;

            Debug.Log("Returned player to: " + player.transform.position);

            DoorSpawnContext.HasReturnPosition = false;
            DoorSpawnContext.NextSpawnId = null;

            return;
        }

        // Normal door spawn
        if (string.IsNullOrEmpty(DoorSpawnContext.NextSpawnId)) return;
        if (DoorSpawnContext.NextSpawnId != spawnId) return;

        player.transform.position = transform.position;

        Debug.Log($"Moved {player.name} to {transform.position}");

        StartCoroutine(ClearSpawnIdEndOfFrame());
    }

    private IEnumerator ClearSpawnIdEndOfFrame()
    {
        yield return new WaitForEndOfFrame();
        DoorSpawnContext.NextSpawnId = null;
    }
}
