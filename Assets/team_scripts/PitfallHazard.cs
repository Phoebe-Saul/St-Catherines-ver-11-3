using System.Collections;
using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class TopDownPitfall : MonoBehaviour
{
    [Header("Forgiving Mash Settings")]
    [Tooltip("Progress gained per button press (0.3 = ~4 presses to escape)")]
    [SerializeField] private float mashGainPerPress = 0.3f;
    [Tooltip("Max simultaneous key presses counted in a single frame")]
    [SerializeField] private int maxCountedPressesPerFrame = 2;
    [Tooltip("Progress lost per second when not mashing (0 = never loses progress)")]
    [SerializeField] private float decayPerSecond = 0f;
    [Tooltip("Guarantees escape within this many seconds even with minimal mashing")]
    [SerializeField] private float maxTrapDuration = 2.5f;

    [Header("Timing & References")]
    [SerializeField] private float fallDuration = 0.15f;
    [SerializeField] private float exitCooldown = 0.75f;
    [SerializeField] private Transform respawnPoint;

    private float escapeProgress;
    private bool isFalling;
    private bool isEscaping;
    private float cooldownTimer;

    private GameObject trappedPlayer;
    private Rigidbody2D playerRb;
    private Behaviour[] movementScriptsToDisable;

    public float EscapeProgress => escapeProgress;

    private void Update()
    {
        if (cooldownTimer > 0f)
        {
            cooldownTimer -= Time.deltaTime;
        }

        if (trappedPlayer == null || isEscaping)
        {
            return;
        }

        // Count presses immediately—even while still in the fall animation
        int presses = Mathf.Min(GetMashPressCount(), maxCountedPressesPerFrame);
        if (presses > 0)
        {
            escapeProgress += presses * mashGainPerPress;
        }
        else if (decayPerSecond > 0f)
        {
            escapeProgress -= decayPerSecond * Time.deltaTime;
        }

        // Passive safety progress so the player never gets stuck
        if (maxTrapDuration > 0f)
        {
            escapeProgress += (1f / maxTrapDuration) * Time.deltaTime;
        }

        escapeProgress = Mathf.Clamp01(escapeProgress);

        // Wait until the brief fall completes before popping out
        if (escapeProgress >= 1f && !isFalling)
        {
            StartCoroutine(EscapePitRoutine());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isFalling || isEscaping || cooldownTimer > 0f || trappedPlayer != null)
        {
            return;
        }

        if (!other.CompareTag("Player"))
        {
            return;
        }

        StartCoroutine(FallIntoPitRoutine(other.gameObject));
    }

    private IEnumerator FallIntoPitRoutine(GameObject player)
    {
        isFalling = true;
        trappedPlayer = player;
        escapeProgress = 0f;

        playerRb = player.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            playerRb.linearVelocity = Vector2.zero;
            playerRb.simulated = false;
        }

        movementScriptsToDisable = player.GetComponents<Behaviour>();
        foreach (var comp in movementScriptsToDisable)
        {
            if (comp != null && comp.enabled && comp.GetType().Name.ToLower().Contains("move"))
            {
                comp.enabled = false;
            }
        }

        player.transform.position = transform.position;
        yield return new WaitForSeconds(fallDuration);

        isFalling = false;

        // If the player already mashed to 100% during the fall, escape right away
        if (escapeProgress >= 1f && !isEscaping)
        {
            StartCoroutine(EscapePitRoutine());
        }
    }

    private IEnumerator EscapePitRoutine()
    {
        isEscaping = true;

        if (trappedPlayer != null)
        {
            Vector3 targetPos = respawnPoint != null ? respawnPoint.position : transform.position;
            trappedPlayer.transform.position = targetPos;

            if (playerRb != null)
            {
                playerRb.simulated = true;
                playerRb.linearVelocity = Vector2.zero;
            }

            if (movementScriptsToDisable != null)
            {
                foreach (var comp in movementScriptsToDisable)
                {
                    if (comp != null)
                    {
                        comp.enabled = true;
                    }
                }
            }
        }

        trappedPlayer = null;
        playerRb = null;
        movementScriptsToDisable = null;
        escapeProgress = 0f;
        cooldownTimer = exitCooldown;
        isEscaping = false;
        yield break;
    }

    private int GetMashPressCount()
    {
        int count = 0;

#if ENABLE_INPUT_SYSTEM
        if (Keyboard.current != null)
        {
            foreach (var key in Keyboard.current.allKeys)
            {
                if (key != null && key.wasPressedThisFrame)
                {
                    count++;
                }
            }
        }

        if (Gamepad.current != null)
        {
            var gp = Gamepad.current;
            if (gp.buttonSouth.wasPressedThisFrame) count++;
            if (gp.buttonEast.wasPressedThisFrame) count++;
            if (gp.buttonWest.wasPressedThisFrame) count++;
            if (gp.buttonNorth.wasPressedThisFrame) count++;
            if (gp.dpad.up.wasPressedThisFrame) count++;
            if (gp.dpad.down.wasPressedThisFrame) count++;
            if (gp.dpad.left.wasPressedThisFrame) count++;
            if (gp.dpad.right.wasPressedThisFrame) count++;
            if (gp.leftShoulder.wasPressedThisFrame) count++;
            if (gp.rightShoulder.wasPressedThisFrame) count++;
        }
#else
        if (Input.anyKeyDown && !Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1))
        {
            count = 1;
        }
#endif

        return count;
    }
}