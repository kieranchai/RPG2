using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopupController: MonoBehaviour
{
    public static PopupController Popup { get; private set; }

    [SerializeField] private TMP_Text popupText;

    private void Awake()
    {
        if (Popup != null && Popup != this)
        {
            Destroy(gameObject);
            return;
        }
        Popup = this;
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
        popupText.text = "WASTED!";
    }

}
