
using UnityEngine;
using System.Collections;

public class ThroneBossAI : MonoBehaviour
{
    [Header("Player")]
    [SerializeField] private Transform player;
    [SerializeField] private float chaseSpeed = 5f;

    [Header("Boss Phases")]
    [SerializeField] private float chaseDuration = 10f;
    [SerializeField] private float attackDuration = 5f;
    [SerializeField] private float timeBetweenBursts = 2f;
    [SerializeField] private int projectilesPerBurst = 3;

    [Header("Projectile")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float projectileSpeed = 10f;
    [SerializeField] private float timeBetweenShots = 0.2f;

    [Header("Pillars")]
    [SerializeField] private BossPillar[] pillars;

    private Rigidbody2D rb;
    private bool isAttacking;
    private bool isDead;
    private int destroyedPillars;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        GameObject playerObject =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
            player = playerObject.transform;
    }

    void Start()
    {
        StartCoroutine(BossFight());
    }

    void FixedUpdate()
    {
        if (player == null || isAttacking || isDead)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction =
            ((Vector2)player.position - rb.position).normalized;

        rb.linearVelocity = direction * chaseSpeed;

        float angle =
            Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    IEnumerator BossFight()
    {
        while (!isDead)
        {
            // PHASE 1: Chase Mary
            isAttacking = false;
            Debug.Log("Boss is chasing Mary!");

            yield return new WaitForSeconds(chaseDuration);

            // PHASE 2: Stop and attack
            isAttacking = true;
            rb.linearVelocity = Vector2.zero;

            Debug.Log("Boss is attacking!");

            yield return StartCoroutine(AttackBurst());

            // PHASE 3: Wait before chasing again
            yield return new WaitForSeconds(attackDuration);
        }
    }

    IEnumerator AttackBurst()
    {
        for (int i = 0; i < projectilesPerBurst; i++)
        {
            if (isDead)
                yield break;

            ShootAtPlayer();

            yield return new WaitForSeconds(timeBetweenShots);
        }
    }

    void ShootAtPlayer()
    {
        if (player == null || projectilePrefab == null)
            return;

        Vector3 spawnPos =
            firePoint != null ? firePoint.position : transform.position;

        GameObject proj =
            Instantiate(projectilePrefab, spawnPos, Quaternion.identity);

        Vector2 direction =
            ((Vector2)player.position - (Vector2)spawnPos).normalized;

        Projectile projectileScript = proj.GetComponent<Projectile>();

        if (projectileScript != null)
            projectileScript.SetDirection(direction, projectileSpeed);
    }

    public void PillarDestroyed()
    {
        destroyedPillars++;

        Debug.Log("Pillars destroyed: " +
            destroyedPillars + " / " + pillars.Length);

        if (destroyedPillars >= pillars.Length)
        {
            Die();
        }
    }

    public void TakePillarDamage()
    {
        Debug.Log("The boss was damaged by a pillar hit!");
    }

    void Die()
    {
        if (isDead)
            return;

        isDead = true;
        rb.linearVelocity = Vector2.zero;

        Debug.Log("THRONE BOSS DEFEATED!");

        // Later we can add a death animation or scene transition.
        gameObject.SetActive(false);
    }
}
