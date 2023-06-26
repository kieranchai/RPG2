using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExperienceBarScript : MonoBehaviour
{
    // Start is called before the first frame update

    // Update is called once per frame
    public Slider slider;
    public Gradient gradient;
    public TMP_Text respectText;
    public void SetExperience(int experience) {
        slider.maxValue = ModifierController.Modifier.xpNeeded;
        slider.value = experience;
    } 
    
    public void SetRespect(int experience) {
        respectText.text = "RESPECT " + experience.ToString("0");
    }
}
