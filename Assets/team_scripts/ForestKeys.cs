
using UnityEngine;

public class ForestKeys : MonoBehaviour
{
    private bool collected = false;
    private Vector3 originalPosition;

    void Awake()
    {
        originalPosition = transform.position;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !collected)
        {
            collected = true;

            KeyManager.Instance.CollectKey();

            gameObject.SetActive(false);
        }
    }

    public void ResetKey()
    {
        transform.position = originalPosition;
        collected = false;
        gameObject.SetActive(true);
    }
}
