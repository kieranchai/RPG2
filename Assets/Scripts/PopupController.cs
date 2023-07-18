using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// KIERAN AND JOEL

public class PopupController: MonoBehaviour
{
    public static PopupController Popup { get; private set; }

    [SerializeField] private TMP_Text popupText;

    private string[] deathTexts;
    private string[] deathSounds;

    private void Awake()
    {
        if (Popup != null && Popup != this)
        {
            Destroy(gameObject);
            return;
        }
        Popup = this;

        this.deathTexts = new string[] { "WASTED!", "SHOCKING!" };
        this.deathSounds = new string[] { "WASTED!", "SHOCKING!", "OH NO YOU DIED!", "TRY HARDER!" };
    }


    public void UpdatePopUp(string text)
    {
        StopCoroutine(Wait());
        popupText.text = text;
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(3);
        popupText.text = "";
    }

    public void SetDeathPopUp()
    {
        StopCoroutine(Wait());
        string deathText = this.deathTexts[Random.Range(0,this.deathTexts.Length)];
        AudioManager.instance.PlaySFX(this.deathSounds[Random.Range(0,this.deathSounds.Length)]);
        popupText.text = deathText;
    }

}
