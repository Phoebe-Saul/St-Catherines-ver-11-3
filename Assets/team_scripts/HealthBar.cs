
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private PlayerHealth playerHealth;

    void Update()
    {
        if (playerHealth != null && slider != null)
        {
            slider.value = playerHealth.currentHealth;
        }
    }
}
