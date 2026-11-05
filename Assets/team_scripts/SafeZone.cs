
using UnityEngine;

public class SafeZone : MonoBehaviour
{
    public static bool IsPlayerSafe { get; private set; }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            IsPlayerSafe = true;
            Debug.Log("Mary entered the safe zone!");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            IsPlayerSafe = false;
            Debug.Log("Mary left the safe zone!");
        }
    }
}
