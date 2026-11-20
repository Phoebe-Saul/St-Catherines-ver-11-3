using System.Collections;
using UnityEngine;

public class SpawnOutside : MonoBehaviour
{
    [SerializeField] private string spawnId;

    private void Start()
    {
        Debug.Log($"SpawnOutside '{spawnId}' | NextSpawnId: '{DoorSpawnContext.NextSpawnId}' | Player: {GameObject.FindGameObjectWithTag("Player")}");

        if (string.IsNullOrEmpty(DoorSpawnContext.NextSpawnId)) return;
        if (DoorSpawnContext.NextSpawnId != spawnId) return;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            player.transform.position = transform.position;
            Debug.Log($"Moved {player.name} to {player.transform.position} (SpawnOutside pos: {transform.position})");
        }

        StartCoroutine(ClearSpawnIdEndOfFrame());
    }

    private IEnumerator ClearSpawnIdEndOfFrame()
    {
        yield return new WaitForEndOfFrame();
        DoorSpawnContext.NextSpawnId = null;
    }
}