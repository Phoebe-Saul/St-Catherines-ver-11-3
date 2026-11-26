using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SixTileDoorInteract : MonoBehaviour
{
    [Serializable]
    public struct DoorTilePiece
    {
        public Vector3Int cellPosition;
        public TileBase closedTile;
        public TileBase openTile;
    }

    [Header("Door Tiles")]
    [SerializeField] private Tilemap doorTilemap;
    [SerializeField] private DoorTilePiece[] doorPieces;
    [SerializeField] private bool isOpen;

    [Header("Clear Exit Points")]
    [SerializeField] private Transform frontExitPoint;
    [SerializeField] private Transform backExitPoint;

    [Header("Interaction")]
    [SerializeField] private KeyCode interactKey = KeyCode.E;
    [SerializeField] private string playerTag = "Player";

    private bool playerInRange;
    private Collider2D playerColliderInTrigger;

    private void Start()
    {
        if (doorTilemap == null)
        {
            doorTilemap = GetComponentInParent<Tilemap>();
        }

        ApplyState();
    }

    private void Update()
    {
        if (Input.GetKeyDown(interactKey))
        {
            Debug.Log($"Pressed {interactKey} | playerInRange = {playerInRange}");
        }

        if (playerInRange && Input.GetKeyDown(interactKey))
        {
            ToggleDoor();
        }
    }

    public void SetPlayerInRange(bool inRange)
    {
        playerInRange = inRange;
    }

    [ContextMenu("Toggle Door")]
    public void ToggleDoor()
    {
        bool opening = !isOpen;

        // Clear the player before closing so they don't get boxed into the closed door tiles.
        if (!opening)
        {
            ClearPlayerFromDoorway();
        }

        isOpen = opening;
        ApplyState();
    }

    private void ClearPlayerFromDoorway()
    {
        if (playerColliderInTrigger == null)
        {
            Debug.LogWarning("Close triggered, but playerColliderInTrigger is null!");
            return;
        }

        Transform playerRoot = playerColliderInTrigger.attachedRigidbody != null
            ? playerColliderInTrigger.attachedRigidbody.transform
            : playerColliderInTrigger.transform;

        Vector2 targetPos = GetPreferredExitPoint(playerRoot.position);
        Debug.Log($"Teleporting {playerRoot.name} from {playerRoot.position} to {targetPos}");

        Rigidbody2D rb = playerColliderInTrigger.attachedRigidbody;
        if (rb != null)
        {
            rb.position = targetPos;
            rb.linearVelocity = Vector2.zero;
            Physics2D.SyncTransforms();
        }

        playerRoot.position = targetPos;
    }

    private Vector2 GetPreferredExitPoint(Vector2 playerPos)
    {
        if (frontExitPoint != null && backExitPoint != null)
        {
            float distToFront = Vector2.SqrMagnitude(playerPos - (Vector2)frontExitPoint.position);
            float distToBack = Vector2.SqrMagnitude(playerPos - (Vector2)backExitPoint.position);
            return distToBack < distToFront ? backExitPoint.position : frontExitPoint.position;
        }

        if (backExitPoint != null)
            return backExitPoint.position;

        if (frontExitPoint != null)
            return frontExitPoint.position;

        return playerPos;
    }

    private void ApplyState()
    {
        if (doorTilemap == null || doorPieces == null) return;

        foreach (DoorTilePiece piece in doorPieces)
        {
            TileBase targetTile = isOpen ? piece.openTile : piece.closedTile;
            doorTilemap.SetTile(piece.cellPosition, targetTile);
            doorTilemap.RefreshTile(piece.cellPosition);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInRange = true;
            playerColliderInTrigger = other;
            Debug.Log("Player entered door trigger");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            playerInRange = false;
            if (playerColliderInTrigger == other)
            {
                playerColliderInTrigger = null;
            }
            Debug.Log("Player exited door trigger");
        }
    }
}