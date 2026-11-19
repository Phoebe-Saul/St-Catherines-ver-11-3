
using UnityEngine;
using System.Collections;

public class ClergymanAI : MonoBehaviour
{
    [Header("Patrol")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float patrolSpeed = 1.5f;
    [SerializeField] private float waypointDistance = 0.15f;

    [Header("Player Detection")]
    [SerializeField] private Transform player;
    [SerializeField] private float detectionRange = 7f;
    [SerializeField] private float lostPlayerTime = 2f;

    [Header("Staff Attack")]
    [SerializeField] private float attackRange = 2.5f;
    [SerializeField] private int attackDamage = 5;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float windupTime = 0.7f;
    [SerializeField] private float recoveryTime = 0.4f;

    [Header("Knockback")]
    [SerializeField] private float knockbackForce = 8f;
    [SerializeField] private float stunDuration = 0.4f;

    [Header("Staff Visual")]
    [SerializeField] private Transform staffTransform;

    private Walking playerWalking;
    private PlayerHealth playerHealth;
    private Rigidbody2D playerRb;

    private int currentWaypoint = 0;

    private bool isChasing = false;
    private bool isAttacking = false;

    private float lostPlayerTimer = 0f;
    private float nextAttackTime = 0f;

    void Awake()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody2D>();

        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;

            playerWalking = playerObject.GetComponent<Walking>();
            playerHealth = playerObject.GetComponent<PlayerHealth>();
            playerRb = playerObject.GetComponent<Rigidbody2D>();
        }
    }

    void Update()
    {
        if (SafeZone.IsPlayerSafe)
        {
            isChasing = false;
            lostPlayerTimer = 0f;
            return;
        }

        if (player == null)
            return;

        float distance =
            Vector2.Distance(transform.position, player.position);

        if (distance <= detectionRange)
        {
            isChasing = true;
            lostPlayerTimer = 0f;
        }
        else if (isChasing)
        {
            lostPlayerTimer += Time.deltaTime;

            if (lostPlayerTimer >= lostPlayerTime)
            {
                isChasing = false;
                lostPlayerTimer = 0f;
            }
        }
    }

    void FixedUpdate()
    {
        if (SafeZone.IsPlayerSafe || isAttacking)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (isChasing)
        {
            ChasePlayer();
            return;
        }

        Patrol();
    }

    void Patrol()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Transform target = patrolPoints[currentWaypoint];

        if (target == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction =
            (Vector2)target.position - rb.position;

        if (direction.magnitude <= waypointDistance)
        {
            currentWaypoint =
                (currentWaypoint + 1) % patrolPoints.Length;

            target = patrolPoints[currentWaypoint];

            if (target == null)
                return;

            direction = (Vector2)target.position - rb.position;
        }

        if (direction.sqrMagnitude > 0.001f)
        {
            rb.linearVelocity = direction.normalized * patrolSpeed;
            FaceDirection(direction);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void ChasePlayer()
    {
        if (player == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction =
            (Vector2)player.position - rb.position;

        float distance = direction.magnitude;

        if (distance <= attackRange)
        {
            rb.linearVelocity = Vector2.zero;

            FaceDirection(direction);

            if (Time.time >= nextAttackTime)
            {
                StartCoroutine(StaffAttack());
            }

            return;
        }

        direction.Normalize();

        // Slower than regular townspeople
        rb.linearVelocity = direction * 3.5f;

        FaceDirection(direction);
    }

    void FaceDirection(Vector2 direction)
    {
        if (direction.sqrMagnitude < 0.001f)
            return;

        float angle =
            Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (staffTransform != null)
        {
            staffTransform.rotation =
                Quaternion.Euler(0f, 0f, angle);
        }
    }

    
IEnumerator StaffAttack()
    {
        isAttacking = true;
        nextAttackTime = Time.time + attackCooldown;

        rb.linearVelocity = Vector2.zero;

        Debug.Log("Clergyman is preparing a staff attack!");

        // Save the staff's resting rotation
        Quaternion restingRotation = staffTransform != null
            ? staffTransform.localRotation
            : Quaternion.identity;

        // Windup: raise the staff slightly
        if (staffTransform != null)
        {
            Quaternion windupRotation =
                restingRotation * Quaternion.Euler(0f, 0f, 30f);

            yield return RotateStaff(
                restingRotation,
                windupRotation,
                windupTime
            );
        }
        else
        {
            yield return new WaitForSeconds(windupTime);
        }

        // Check whether Mary is still in range
        if (player != null)
        {
            float distance =
                Vector2.Distance(transform.position, player.position);

            if (distance <= attackRange)
            {
                HitPlayer();
                Debug.Log("Clergyman hit Mary!");
            }
            else
            {
                Debug.Log("Clergyman missed!");
            }
        }

        // Swing the staff forward
        if (staffTransform != null)
        {
            Quaternion swingRotation =
                restingRotation * Quaternion.Euler(0f, 0f, -100f);

            yield return RotateStaff(
                staffTransform.localRotation,
                swingRotation,
                0.15f
            );

            // Return staff to resting position
            yield return RotateStaff(
                staffTransform.localRotation,
                restingRotation,
                0.2f
            );
        }

        yield return new WaitForSeconds(recoveryTime);

        isAttacking = false;
    }

    IEnumerator RotateStaff(
        Quaternion start,
        Quaternion end,
        float duration)
    {
        if (staffTransform == null)
            yield break;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float progress = Mathf.Clamp01(elapsed / duration);

            staffTransform.localRotation =
                Quaternion.Slerp(start, end, progress);

            yield return null;
        }

        staffTransform.localRotation = end;
    }


    void HitPlayer()
    {
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(attackDamage);
        }

        Debug.Log("Mary was hit by the clergyman's staff!");

        if (playerRb != null)
        {
            Vector2 knockbackDirection =
                ((Vector2)player.position - rb.position).normalized;

            StartCoroutine(ApplyKnockback(knockbackDirection));
        }
    }

    IEnumerator ApplyKnockback(Vector2 direction)
    {
        bool movementWasEnabled =
            playerWalking != null && playerWalking.enabled;

        // Temporarily disable Mary's movement
        if (movementWasEnabled)
            playerWalking.enabled = false;

        playerRb.linearVelocity = Vector2.zero;

        playerRb.AddForce(
            direction * knockbackForce,
            ForceMode2D.Impulse
        );

        yield return new WaitForSeconds(stunDuration);

        playerRb.linearVelocity = Vector2.zero;

        // Only re-enable movement if it was enabled before
        if (movementWasEnabled && playerWalking != null)
            playerWalking.enabled = true;
    }
}
