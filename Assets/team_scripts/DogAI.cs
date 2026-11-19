
using UnityEngine;
using System.Collections;

public class DogAI : MonoBehaviour
{
    public enum DogState
    {
        Patrol,
        Chase,
        Windup,
        Pounce,
        Recovery
    }

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform player;
    private Walking playerWalking;

    [Header("Patrol")]
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float waypointDistance = 0.15f;

    [Header("Detection")]
    [SerializeField] private float detectionRadius = 6f;
    [SerializeField] private float sneakingDetectionRadius = 2f;
    [SerializeField] private Transform detectionVisual;

    [Header("Chase")]
    [SerializeField] private float chaseSpeed = 3f;
    [SerializeField] private float pounceDistance = 2f;

    [Header("Pounce")]
    [SerializeField] private float windupTime = 0.5f;
    [SerializeField] private float pounceSpeed = 8f;
    [SerializeField] private float pounceDuration = 0.25f;
    [SerializeField] private float recoveryTime = 0.8f;
    [SerializeField] private float catchDistance = 0.5f;

    private DogState currentState = DogState.Patrol;

    private int currentWaypoint;
    private float stateTimer;

    private Vector2 pounceDirection;

    private bool hasCaughtPlayer;

    void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }

        if (player == null)
        {
            GameObject playerObject =
                GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }

        if (player != null)
        {
            playerWalking = player.GetComponent<Walking>();
        }

        Debug.Log("DogAI initialized!");
Debug.Log("Rigidbody: " + (rb != null));
Debug.Log("Player: " + (player != null));
Debug.Log("Walking: " + (playerWalking != null));
Debug.Log("Patrol Points: " + (patrolPoints != null ? patrolPoints.Length : 0));
    }

    void Update()
    {
        UpdateDetectionVisual();

        if (rb == null || player == null || playerWalking == null)
            return;

        //dogs still patrol when player is in safe zone
        if (SafeZone.IsPlayerSafe)
            {
            currentState = DogState.Patrol;
            stateTimer = 0f;
        return;
        }

        if (hasCaughtPlayer)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        float distance = Vector2.Distance(
            transform.position,
            player.position
        );

        float currentRadius = playerWalking.IsSneaking
            ? sneakingDetectionRadius
            : detectionRadius;

        bool playerDetected = distance <= currentRadius;

        switch (currentState)
        {
            case DogState.Patrol:

                if (playerDetected)
                {
                    currentState = DogState.Chase;
                }

                break;

            case DogState.Chase:

                if (!playerDetected)
                {
                    currentState = DogState.Patrol;
                }
                else if (distance <= pounceDistance)
                {
                    currentState = DogState.Windup;
                    stateTimer = windupTime;
                }

                break;

            case DogState.Windup:

                if (!playerDetected)
                {
                    currentState = DogState.Patrol;
                }
                else
                {
                    stateTimer -= Time.deltaTime;

                    if (stateTimer <= 0f)
                    {
                        pounceDirection =
                            ((Vector2)player.position - rb.position).normalized;

                        currentState = DogState.Pounce;
                        stateTimer = pounceDuration;
                    }
                }

                break;

            case DogState.Pounce:

                float pounceDistanceFromPlayer = Vector2.Distance(
                    rb.position,
                    player.position
                );

                if (pounceDistanceFromPlayer <= catchDistance)
                {
                    CatchPlayer();
                    return;
                }

                stateTimer -= Time.deltaTime;

                if (stateTimer <= 0f)
                {
                    currentState = DogState.Recovery;
                    stateTimer = recoveryTime;
                }

                break;

            case DogState.Recovery:

                stateTimer -= Time.deltaTime;

                if (stateTimer <= 0f)
                {
                    currentState = playerDetected
                        ? DogState.Chase
                        : DogState.Patrol;
                }

                break;
        }
    }

    
void FixedUpdate()
{
    if (rb == null)
        return;

    if (hasCaughtPlayer)
{
    rb.linearVelocity = Vector2.zero;
    return;
}

    switch (currentState)
    {
        case DogState.Patrol:
            Patrol();
            break;

        case DogState.Chase:
            ChasePlayer();
            break;

        case DogState.Windup:
        case DogState.Recovery:
            rb.linearVelocity = Vector2.zero;
            break;

        case DogState.Pounce:
            rb.linearVelocity = pounceDirection * pounceSpeed;
            break;
    }
}


    void Patrol()
    {
        Debug.Log("Patrol is executing!");
        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (currentWaypoint >= patrolPoints.Length)
        {
            currentWaypoint = 0;
        }

        Transform target = patrolPoints[currentWaypoint];

        if (target == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction = (Vector2)target.position - rb.position;

        if (direction.magnitude <= waypointDistance)
        {
            currentWaypoint =
                (currentWaypoint + 1) % patrolPoints.Length;

            target = patrolPoints[currentWaypoint];

            if (target == null)
            {
                rb.linearVelocity = Vector2.zero;
                return;
            }

            direction = (Vector2)target.position - rb.position;
        }

        rb.linearVelocity = direction.normalized * patrolSpeed;
    }

    void ChasePlayer()
    {
        if (player == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction =
            ((Vector2)player.position - rb.position).normalized;

        rb.linearVelocity = direction * chaseSpeed;
    }

    void UpdateDetectionVisual()
    {
        if (detectionVisual == null || playerWalking == null)
            return;

        SpriteRenderer visualRenderer =
            detectionVisual.GetComponent<SpriteRenderer>();

        if (visualRenderer == null || visualRenderer.sprite == null)
            return;

        float radius = playerWalking.IsSneaking
            ? sneakingDetectionRadius
            : detectionRadius;

        Vector2 spriteSize = visualRenderer.sprite.bounds.size;

        float diameter = radius * 2f;

        detectionVisual.localScale = new Vector3(
            diameter / spriteSize.x,
            diameter / spriteSize.y,
            1f
        );
    }

    void CatchPlayer()
    {
        if (hasCaughtPlayer)
            return;

        hasCaughtPlayer = true;

        rb.linearVelocity = Vector2.zero;

        Debug.Log("DOG HAS CAUGHT THE PLAYER!");

        if (CatchManager.Instance != null)
        {
            CatchManager.Instance.PlayerCaught();
        }

        StartCoroutine(ResetCatch());
    }

    IEnumerator ResetCatch()
    {
        yield return new WaitForSeconds(2f);

        hasCaughtPlayer = false;
    }
}
