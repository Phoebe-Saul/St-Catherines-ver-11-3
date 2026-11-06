
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifetime = 5f;

    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public void SetDirection(Vector2 direction, float speed)
    {
        if (rb != null)
        {
            rb.linearVelocity = direction * speed;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Ignore the boss that fired the projectile
        if (collision.CompareTag("ThroneBoss"))
            return;

        // Hit a pillar
        BossPillar pillar = collision.GetComponent<BossPillar>();

        if (pillar != null)
        {
            pillar.TakeHit();
            Destroy(gameObject);
            return;
        }

        // Hit Mary
        if (collision.CompareTag("Player"))
        {
            PlayerHealth health = collision.GetComponent<PlayerHealth>();

            if (health != null)
            {
                health.TakeDamage(20);
            }

            Destroy(gameObject);
            return;
        }

        // Hit another obstacle
        if (!collision.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}
