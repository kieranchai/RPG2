using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

// KIERAN

public class Notification : MonoBehaviour
{
    [SerializeField] private TMP_Text Name;
    [SerializeField] private TMP_Text Desc;
    string formattedDialogueText;

    private void Start()
    {
        Destroy(gameObject, 3f);
    }

    public void Initialise(string achName, string achDesc, string achValue, string achType)
    {
        this.Name.text = achName;
        if (achType == "TIMEPLAYED")
        {
            int value;
            value = int.Parse(achValue) / 60;
            achValue = value.ToString();
        }

        this.formattedDialogueText = achDesc.Replace("achValue", achValue);

        this.Desc.text = this.formattedDialogueText;
    }
}
