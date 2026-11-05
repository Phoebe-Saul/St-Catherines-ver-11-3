
using UnityEngine;

public class TownspersonAI : MonoBehaviour
{
    [Header("Patrol")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float waypointDistance = 0.15f;
    [SerializeField] private Transform flashlightBeam;

    [Header("Player")]
    [SerializeField] private Transform player;
    private Walking playerWalking;

    [Header("Vision Detection")]
    [SerializeField] private float normalRange = 6f;
    [SerializeField] private float sneakRange = 2f;
    [SerializeField] private float normalAngle = 60f;
    [SerializeField] private float sneakAngle = 35f;

private bool playerDetected;

    [Header("Flashlight Size")]
    [SerializeField] private Vector3 normalBeamScale = new Vector3(6f, 4f, 1f);
    [SerializeField] private Vector3 sneakBeamScale = new Vector3(4f, 3.5f, 1f);

    [Header("Chase")]
    [SerializeField] private float chaseSpeed = 6f;
    [SerializeField] private float grabDistance = 2f;
    [SerializeField] private float lostPlayerTime = 1.5f;

    private float lostPlayerTimer;
    private bool hasCaughtPlayer;
    private bool isChasing;

    private int currentWaypoint = 0;

    void Awake()
    {
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
        }
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.transform;
            playerWalking = playerObject.GetComponent<Walking>();
        }
    }
    void Update()
    {
        UpdateFlashlightSize();

        playerDetected = CanSeePlayer();

        if (playerDetected)
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
        if (isChasing)
        {
            ChasePlayer();
            return;
        }
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
            {
                rb.linearVelocity = Vector2.zero;
                return;
            }

            direction = (Vector2)target.position - rb.position;
        }

        if (direction.sqrMagnitude > 0.001f)
        {
            rb.linearVelocity = direction.normalized * patrolSpeed;

            if (flashlightBeam != null)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                flashlightBeam.rotation = Quaternion.Euler(0f, 0f, angle);
            }
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }

    }
    void UpdateFlashlightSize()
    {
        if (flashlightBeam == null || playerWalking == null)
            return;

        if (playerWalking.IsSneaking)
        {
            flashlightBeam.localScale = sneakBeamScale;
        }
        else
        {
            flashlightBeam.localScale = normalBeamScale;
        }
    }

    bool CanSeePlayer()
    {
        if (player == null || flashlightBeam == null || playerWalking == null)
            return false;

        Vector2 directionToPlayer =
            (player.position - transform.position).normalized;

        float distanceToPlayer =
            Vector2.Distance(transform.position, player.position);

        float currentRange = playerWalking.IsSneaking ? sneakRange : normalRange;
        float currentAngle = playerWalking.IsSneaking ? sneakAngle : normalAngle;

        if (distanceToPlayer > currentRange)
            return false;

        float angle = Vector2.Angle(flashlightBeam.right, directionToPlayer);

        if (angle > currentAngle / 2f)
            return false;

        return true;
    }

    
    void ChasePlayer()
    {
        if (player == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction = (player.position - transform.position).normalized;
        
        float distance = Vector2.Distance(rb.position, player.position);
        //Debug.Log("Distance to Mary: " + distance);
        // Catch Mary if close enough
        Debug.Log("Distance: " + distance + " | Grab: " + grabDistance);
        if (distance <= grabDistance)
        {
            Debug.Log("Grab Distance Reached");
            CatchPlayer();
            return;
        }

        rb.linearVelocity = direction * chaseSpeed;

        if (flashlightBeam != null)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            flashlightBeam.rotation = Quaternion.Euler(0f, 0f, angle);
        }   
    }

    
    void CatchPlayer()
    {
        if (hasCaughtPlayer)
            return;

        hasCaughtPlayer = true;

        rb.linearVelocity = Vector2.zero;
        isChasing = false;

        Debug.Log("MARY HAS BEEN CAUGHT!");

        CatchManager.Instance.PlayerCaught();

        // Allow the enemy to catch Mary again
        StartCoroutine(ResetCatch());
    }

    System.Collections.IEnumerator ResetCatch()
    {
        yield return new WaitForSeconds(2f);

        hasCaughtPlayer = false;
    }



}
