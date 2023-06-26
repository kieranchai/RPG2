using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnalyticsController : MonoBehaviour
{
    public static AnalyticsController Analytics { get; private set; }

    public int enemiesKilled = 0;
    public int damageTaken = 0;
    public int damageDealt = 0;
    public int timePlayed = 0;

    private float timer;

    [SerializeField] private GameObject statisticsDisplay;

    private void Awake()
    {
        if (Analytics != null && Analytics != this)
        {
            Destroy(gameObject);
            return;
        }
        Analytics = this;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        timePlayed = (int)(timer % 60); //time played in seconds
        UpdateStatDisplay();
    }

    private void UpdateStatDisplay()
    {
        statisticsDisplay.transform.Find("STATS_EK").GetComponent<Text>().text = this.enemiesKilled.ToString();
        statisticsDisplay.transform.Find("STATS_DT").GetComponent<Text>().text = this.damageTaken.ToString();
        statisticsDisplay.transform.Find("STATS_DD").GetComponent<Text>().text = this.damageDealt.ToString();
        statisticsDisplay.transform.Find("STATS_TP").GetComponent<Text>().text = this.timePlayed.ToString();

    }
}
