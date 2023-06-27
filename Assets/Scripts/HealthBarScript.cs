using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HealthBarScript : MonoBehaviour
{
    public Slider slider;
    void Start()
    {
        // slider.value = PlayerScript.Player.currentHealth;
        // slider.maxValue = PlayerScript.Player.maxHealth;
    }

    public void updateHealthBar() {
        slider.value = PlayerScript.Player.currentHealth;
        slider.maxValue = PlayerScript.Player.maxHealth;
        slider.value = PlayerScript.Player.currentHealth;
    }
}
