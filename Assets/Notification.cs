using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Notification : MonoBehaviour
{
    [SerializeField] private TMP_Text Name;
    [SerializeField] private TMP_Text Desc;

    private void Start()
    {
        Destroy(gameObject, 3f);
    }

    public void Initialise(string achName, string achDesc)
    {
        this.Name.text = achName;
        this.Desc.text = achDesc;
    }
}
