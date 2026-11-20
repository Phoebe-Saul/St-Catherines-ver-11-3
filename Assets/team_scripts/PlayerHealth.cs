
using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Respawn")]
    [SerializeField] private Transform respawnPoint;
    [SerializeField] private float respawnDelay = 2f;
    [SerializeField] private GameObject respawnPanel;

    private Walking playerWalking;
    private Rigidbody2D rb;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;

        playerWalking = GetComponent<Walking>();
        rb = GetComponent<Rigidbody2D>();
        respawnPanel.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log("Mary's health: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount)
    {
        if (isDead) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;

        Debug.Log("MARY DIED!");

        respawnPanel.SetActive(true);

        //StartCoroutine(Respawn());
    }
    public void buttonrespawn(){
        StartCoroutine(Respawn());
    }

    IEnumerator Respawn()
    {
        respawnPanel.SetActive(false);
        // Stop Mary from moving
        if (playerWalking != null)
            playerWalking.enabled = false;

        if (rb != null)
            rb.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(respawnDelay);

        if (respawnPoint != null)
        {
            transform.position = respawnPoint.position;

            if (rb != null)
            {
                rb.position = respawnPoint.position;
                rb.linearVelocity = Vector2.zero;
            }
        }
        else
        {
            Debug.LogError("Respawn Point is not assigned!");
        }

        // Restore health
        currentHealth = maxHealth;

        // Allow Mary to move again
        if (playerWalking != null)
            playerWalking.enabled = true;

        isDead = false;

        Debug.Log("Mary has respawned!");
    }
}
