
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class BearTrapHazard : MonoBehaviour
{
    [Header("Trap Settings")]
    [SerializeField] private int escapePressesRequired = 10;
    [SerializeField] private float damageInterval = 2f;
    [SerializeField] private int damagePerTick = 5;

    [Header("Trap Visuals")]
    [SerializeField] private GameObject openTrap;
    [SerializeField] private GameObject closedTrap;
    [SerializeField] private GameObject spaceBar;

    [Header("Escape UI")]
    [SerializeField] private TMP_Text escapeProgressText;

    [Header("Trap Position")]
    [SerializeField] private Transform trapCenter;

    private Walking trappedPlayer;
    private Rigidbody2D trappedPlayerRb;
    private PlayerHealth trappedPlayerHealth;

    private RigidbodyConstraints2D originalConstraints;

    private int escapePresses;
    private bool playerTrapped = false;

    private float damageTimer;

    void Start()
    {
        // Set the trap's starting appearance
        if (openTrap != null)
            openTrap.SetActive(true);

        if (closedTrap != null)
            closedTrap.SetActive(false);

        if (spaceBar != null)
            spaceBar.SetActive(false);

        if (escapeProgressText != null)
            escapeProgressText.gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (playerTrapped)
            return;

        Walking player = other.GetComponentInParent<Walking>();

        if (player == null)
            return;

        trappedPlayer = player;

        trappedPlayerRb = player.GetComponent<Rigidbody2D>();
        trappedPlayerHealth = player.GetComponent<PlayerHealth>();

        playerTrapped = true;
        escapePresses = 0;
        damageTimer = damageInterval;

        // Move Mary to the center of the trap
        Vector2 centerPosition = trapCenter != null
            ? (Vector2)trapCenter.position
            : (Vector2)transform.position;

        player.transform.position = centerPosition;

        // Switch the trap visuals
        if (openTrap != null)
            openTrap.SetActive(false);

        if (closedTrap != null)
            closedTrap.SetActive(true);

        // Show the escape prompt
        if (spaceBar != null)
            spaceBar.SetActive(true);

        if (escapeProgressText != null)
        {
            escapeProgressText.gameObject.SetActive(true);
            UpdateEscapeText();
        }

        // Stop and freeze Mary
        if (trappedPlayerRb != null)
        {
            originalConstraints = trappedPlayerRb.constraints;

            trappedPlayerRb.position = centerPosition;
            trappedPlayerRb.linearVelocity = Vector2.zero;

            trappedPlayerRb.constraints =
                RigidbodyConstraints2D.FreezeAll;
        }

        // Disable normal movement
        trappedPlayer.enabled = false;

        Debug.Log("Mary is caught! Mash SPACE to escape!");
    }

    private void Update()
    {
        if (!playerTrapped)
            return;

        // Slowly damage Mary while trapped
        damageTimer -= Time.deltaTime;

        if (damageTimer <= 0f)
        {
            if (trappedPlayerHealth != null)
            {
                trappedPlayerHealth.TakeDamage(damagePerTick);

                Debug.Log("Bear trap damaged Mary!");
            }

            damageTimer = damageInterval;
        }

        // Check for Space presses
        if (Keyboard.current != null &&
            Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            escapePresses++;

            Debug.Log("Escape: " + escapePresses +
                "/" + escapePressesRequired);

            UpdateEscapeText();

            if (escapePresses >= escapePressesRequired)
            {
                EscapeTrap();
            }
        }
    }

    private void UpdateEscapeText()
    {
        if (escapeProgressText != null)
        {
            escapeProgressText.text =
                "TRAPPED! MASH SPACE TO ESCAPE!\n" +
                escapePresses + " / " + escapePressesRequired;
        }
    }

    private void EscapeTrap()
    {
        // Restore Mary's physics
        if (trappedPlayerRb != null)
        {
            trappedPlayerRb.linearVelocity = Vector2.zero;

            trappedPlayerRb.constraints = originalConstraints;
        }

        // Restore Mary's movement
        if (trappedPlayer != null)
        {
            trappedPlayer.enabled = true;
        }

        // Switch trap visuals back
        if (openTrap != null)
            openTrap.SetActive(true);

        if (closedTrap != null)
            closedTrap.SetActive(false);

        // Hide escape prompt
        if (spaceBar != null)
            spaceBar.SetActive(false);

        if (escapeProgressText != null)
            escapeProgressText.gameObject.SetActive(false);

        trappedPlayer = null;
        trappedPlayerRb = null;
        trappedPlayerHealth = null;

        playerTrapped = false;
        escapePresses = 0;

        Debug.Log("Mary escaped!");
    }
}
