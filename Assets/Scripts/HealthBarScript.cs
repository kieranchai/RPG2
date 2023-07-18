using UnityEngine;
using UnityEngine.UI;

// JOEL

public class HealthBarScript : MonoBehaviour
{
    public Slider slider;

    public void UpdateHealthBar() {
        slider.value = PlayerScript.Player.currentHealth;
        slider.maxValue = PlayerScript.Player.maxHealth;
        slider.value = PlayerScript.Player.currentHealth;
    }
}
