using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// KIERAN

public class SlidingNumber : MonoBehaviour
{
    public TMP_Text numberText;
    public float animationTime = 1.5f;

    private float desiredNumber;
    private float initialNumber;
    private float currentNumber;

    public void SetNumber(int value)
    {
        initialNumber = currentNumber;
        desiredNumber = value;
    }

    public void AddToNumber(int value)
    {
        initialNumber = currentNumber;
        desiredNumber += value;
    }

    private void Update()
    {
        if (currentNumber != desiredNumber)
        {
            if (initialNumber < desiredNumber)
            {
                currentNumber += (animationTime * Time.deltaTime) * (desiredNumber - initialNumber);
                if (currentNumber >= desiredNumber)
                {
                    currentNumber = desiredNumber;
                }
            }
            else
            {
                currentNumber -= (animationTime * Time.deltaTime) * (initialNumber - desiredNumber);
                if (currentNumber <= desiredNumber)
                {
                    currentNumber = desiredNumber;
                }
            }

            numberText.text = "$" + currentNumber.ToString("0");
        }
    }
}
