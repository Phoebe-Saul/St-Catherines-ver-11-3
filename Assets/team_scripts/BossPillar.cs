
using UnityEngine;

public class BossPillar : MonoBehaviour
{
    [Header("Pillar Health")]
    [SerializeField] private int hitsToDestroy = 3;

    [Header("Boss")]
    [SerializeField] private ThroneBossAI boss;

    private int currentHealth;

    void Awake()
    {
        currentHealth = hitsToDestroy;
    }

    public void TakeHit()
    {
        currentHealth--;

        Debug.Log(gameObject.name +
            " was hit! Health: " + currentHealth);

        if (boss != null)
        {
            boss.TakePillarDamage();
        }

        if (currentHealth <= 0)
        {
            DestroyPillar();
        }
    }

    void DestroyPillar()
    {
        Debug.Log(gameObject.name + " was destroyed!");

        if (boss != null)
        {
            boss.PillarDestroyed();
        }

        Destroy(gameObject);
    }
}
