using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    private float duration;

    [SerializeField] private Scrollbar myScroll;

    // Fake loader lol
    void Start()
    {
        duration = 5.0f;
        StartCoroutine(Load());
    }

    IEnumerator Load()
    {
        float timer = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            myScroll.size = (timer/duration);
            yield return null;
        }
        SceneManager.LoadScene("Character Selection");
    }
}
