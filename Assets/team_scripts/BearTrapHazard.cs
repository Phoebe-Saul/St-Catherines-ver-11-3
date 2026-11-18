
using UnityEngine;
using UnityEngine.InputSystem;

public class BearTrapHazard : MonoBehaviour
{
    [Header("Trap Settings")]
    [SerializeField] private int escapePressesRequired = 10;
    [SerializeField] private float damageAmount = 10f;

    private Walking trappedPlayer;
    private Rigidbody2D trappedPlayerRb;

    private RigidbodyConstraints2D originalConstraints;

    private int escapePresses;
    private bool playerTrapped = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (playerTrapped)
            return;

        Walking player = other.GetComponent<Walking>();

        if (player == null)
            return;

        trappedPlayer = player;

        // IMPORTANT: Actually get Mary's Rigidbody2D!
        trappedPlayerRb = player.GetComponent<Rigidbody2D>();

        playerTrapped = true;
        escapePresses = 0;

        // Save original physics settings
        if (trappedPlayerRb != null)
        {
            originalConstraints = trappedPlayerRb.constraints;

            // Stop all movement
            trappedPlayerRb.linearVelocity = Vector2.zero;
            trappedPlayerRb.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        // Disable movement script
        trappedPlayer.enabled = false;

        Debug.Log("Mary is caught! Mash SPACE to escape!");
    }

    private void Update()
    {
        if (!playerTrapped)
            return;

        if (Keyboard.current != null &&
            Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            escapePresses++;

            Debug.Log("Escape: " + escapePresses +
                "/" + escapePressesRequired);

            if (escapePresses >= escapePressesRequired)
            {
                EscapeTrap();
            }
        }
    }

    private void EscapeTrap()
    {
        if (trappedPlayerRb != null)
        {
            trappedPlayerRb.linearVelocity = Vector2.zero;
            trappedPlayerRb.constraints = originalConstraints;
        }

        if (trappedPlayer != null)
        {
            trappedPlayer.enabled = true;
        }

        trappedPlayer = null;
        trappedPlayerRb = null;

        playerTrapped = false;
        escapePresses = 0;

        Debug.Log("Mary escaped!");
    }
}
