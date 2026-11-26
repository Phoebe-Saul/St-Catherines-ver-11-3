using UnityEngine;

public class DoorEjectPlayer : MonoBehaviour
{
    [Header("Doorway Clearance Check")]
    [Tooltip("Optional: drag a child/marker placed right over the doorway if the Door folder isn't centered there.")]
    [SerializeField] private Transform doorwayCenter;
    [SerializeField] private Vector2 checkOffset = Vector2.zero;
    [SerializeField] private Vector2 checkSize = new Vector2(1.5f, 1.5f);
    [SerializeField] private LayerMask playerLayer;

    [Header("Clear Exit Points")]
    [SerializeField] private Transform frontExitPoint;
    [SerializeField] private Transform backExitPoint;

    [Header("Debug")]
    [SerializeField] private bool logDebug = true;

    [SerializeField] private bool isOpen = true;

    public void SetOpenState(bool open)
    {
        isOpen = open;
    }

    public void CloseDoorAndClearPlayer()
    {
        if (logDebug)
            Debug.Log($"[{name}] CloseDoorAndClearPlayer called. isOpen={isOpen}", this);

        if (!isOpen)
            return;

        Vector2 origin = doorwayCenter != null ? (Vector2)doorwayCenter.position : (Vector2)transform.position;
        Vector2 boxCenter = origin + checkOffset;

        Collider2D playerHit = Physics2D.OverlapBox(boxCenter, checkSize, 0f, playerLayer);

        if (logDebug)
            Debug.Log($"[{name}] OverlapBox at {boxCenter} size {checkSize} hit: {(playerHit != null ? playerHit.name : "NONE")}", this);

        if (playerHit != null)
        {
            MovePlayerToClearSide(playerHit);
        }

        // Move the player first, then apply closed door tiles/colliders.
        isOpen = false;
        ApplyClosedDoorVisualsAndCollision();
    }

    private void MovePlayerToClearSide(Collider2D playerCollider)
    {
        Transform playerRoot = playerCollider.attachedRigidbody != null
            ? playerCollider.attachedRigidbody.transform
            : playerCollider.transform;

        Vector2 currentPos = playerRoot.position;
        Vector2 targetPos = GetPreferredExitPoint(currentPos);

        if (logDebug)
            Debug.Log($"[{name}] Ejecting {playerRoot.name} from {currentPos} to {targetPos}", this);

        Rigidbody2D rb = playerCollider.attachedRigidbody;
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

    private void ApplyClosedDoorVisualsAndCollision()
    {
        // Hook your closed door tile placement / collider activation here.
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Vector2 origin = doorwayCenter != null ? (Vector2)doorwayCenter.position : (Vector2)transform.position;
        Gizmos.DrawWireCube(origin + checkOffset, checkSize);
    }
}