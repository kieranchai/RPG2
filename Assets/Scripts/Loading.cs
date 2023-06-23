using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    public float duration;

    [SerializeField] private Scrollbar scroll;

    // Fake loader lol
    void Start()
    {
        StartCoroutine(Load());
    }

    IEnumerator Load()
    {
        float timer = 0;
        while (timer < duration)
        {
            timer += Time.deltaTime;
            scroll.size = (timer/duration);
            yield return null;
        }
        SceneManager.LoadScene("Character Selection");
    }
}
