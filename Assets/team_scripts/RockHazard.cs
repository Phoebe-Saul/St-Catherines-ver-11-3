
using UnityEngine;

public class RockHazard : MonoBehaviour
{
    [Header("Slowdown")]
    [SerializeField] private float slowMultiplier = 0.5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Walking player = other.GetComponent<Walking>();

        if (player != null)
        {
            player.SetHazardSlow(slowMultiplier);
            Debug.Log("Mary stepped on a sharp rock!");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Walking player = other.GetComponent<Walking>();

        if (player != null)
        {
            player.SetHazardSlow(1f);
            Debug.Log("Mary escaped the sharp rock!");
        }
    }
}
