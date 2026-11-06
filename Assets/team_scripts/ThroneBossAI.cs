
using UnityEngine;

public class ThroneBossAI : MonoBehaviour
{
    [Header("Player Tracking")]
    [SerializeField] private Transform player;
    [SerializeField] private float chaseSpeed = 5f;

    [Header("Projectile")]
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireInterval = 1.5f;
    [SerializeField] private float projectileSpeed = 10f;

    private Rigidbody2D rb;
    private float fireTimer;

    void Awake(){
        rb = GetComponent<Rigidbody2D>();

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if(playerObject != null){
            player = playerObject.transform;
        }
    }

    void Update(){
        if(player == null) return;

        fireTimer += Time.deltaTime;
        if(fireTimer >= fireInterval){
            ShootAtPlayer();
            fireTimer = 0f;
        }        
   }

   void FixedUpdate(){
        if(player == null){
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 direction = ((Vector2)player.position - rb.position).normalized;
        rb.linearVelocity = direction * chaseSpeed;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
   }

   void ShootAtPlayer(){
        if (projectilePrefab == null) return;

        Vector3 spawnPos = firePoint != null ? firePoint.position : transform.position;
        GameObject proj = Instantiate(projectilePrefab, spawnPos, Quaternion.identity);

        //automatically ignore collision between projectile and boss
        Collider2D bossCollider = GetComponent<Collider2D>();
        Collider2D projCollider = proj.GetComponent<Collider2D>();
        if(bossCollider != null && projCollider != null){
            Physics2D.IgnoreCollision(bossCollider, projCollider);
        }

        //calculate direction towards player
        Vector2 shootDirection = ((Vector2)player.position - (Vector2)spawnPos).normalized;

        Rigidbody2D projRb = proj.GetComponent<Rigidbody2D>();
        if(projRb != null){
            projRb.linearVelocity = shootDirection * projectileSpeed;
        }
   }

   void OnTriggerEnter2D(Collider2D collision){
        if(collision.CompareTag("Player")){
            Debug.Log("ENTERED AOE");
            //add damage stuff here later
        }
   }

}
