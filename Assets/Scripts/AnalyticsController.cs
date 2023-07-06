using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AnalyticsController : MonoBehaviour
{
    public static AnalyticsController Analytics { get; private set; }

    public int enemiesKilled = 0;
    public float damageTaken = 0;
    public float damageDealt = 0;
    public int timePlayed = 0;
    public int experienceGained = 0;
    public int questCompleted = 0;
    public Achievement[] allAchievement;
    public Achievement[] questAchievement;
    public Achievement[] killAchievement;
    public Achievement[] timeAchievment;
    public Achievement[] weaponAchievment;

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

        allAchievement = Resources.LoadAll<Achievement>("ScriptableObjects/Achievement");
        Array.Sort(allAchievement, (a, b) => a.achId - b.achId);

        foreach (Achievement ach in allAchievement) {
            ach.isCompleted = false;
        }

        questAchievement = Array.FindAll(allAchievement, element => element.achType == "QUEST");
        Array.Sort(questAchievement, (a, b) => a.achId - b.achId);

        killAchievement = Array.FindAll(allAchievement, element => element.achType == "KILL");
        Array.Sort(killAchievement, (a, b) => a.achId - b.achId);

        timeAchievment = Array.FindAll(allAchievement, element => element.achType == "TIMEPLAYED");
        Array.Sort(timeAchievment, (a, b) => a.achId - b.achId);

        weaponAchievment = Array.FindAll(allAchievement, element => element.achType == "WEAPON");
        Array.Sort(weaponAchievment, (a, b) => a.achId - b.achId);
    }

    private void Update()
    {
        if (GameControllerScript.GameController.isAlive) timer += Time.deltaTime;
        timePlayed = (int)(timer % 60); //time played in seconds
        UpdateStatDisplay();
        CheckAchievements("TIME");
    }

    private void UpdateStatDisplay()
    {
        statisticsDisplay.transform.Find("STATS_EK").GetComponent<Text>().text = this.enemiesKilled.ToString();
        statisticsDisplay.transform.Find("STATS_DT").GetComponent<Text>().text = this.damageTaken.ToString();
        statisticsDisplay.transform.Find("STATS_DD").GetComponent<Text>().text = this.damageDealt.ToString();
        statisticsDisplay.transform.Find("STATS_TP").GetComponent<Text>().text = this.timePlayed.ToString();
        statisticsDisplay.transform.Find("STATS_XP").GetComponent<Text>().text = this.experienceGained.ToString();
        statisticsDisplay.transform.Find("STATS_QC").GetComponent<Text>().text = this.questCompleted.ToString();
    }

    public void CheckAchievements(string type)
    {
        switch (type)
        {
            case ("KILL"):
                Debug.Log("Check achkill");
                foreach (Achievement ach in killAchievement)
                {
                    // Debug.Log(ach.achName);
                    if (!ach.isCompleted)
                    {
                        if (enemiesKilled >= int.Parse(ach.achValue))
                        {
                            ach.isCompleted = true;
                            Debug.Log(ach.achName + " IS COMPLETED");
                            //display popup
                        }
                    }
                }
                break;
            case ("QUEST"):
                foreach (Achievement ach in questAchievement)
                {
                    if (!ach.isCompleted)
                    {
                        if (questCompleted >= int.Parse(ach.achValue))
                        {
                            ach.isCompleted = true;
                            Debug.Log(ach.achName + " IS COMPLETED");
                        }

                    }
                }
                break;
            case ("TIME"):
                foreach (Achievement ach in timeAchievment) {
                    if (!ach.isCompleted) {
                        if (this.timePlayed >= int.Parse(ach.achValue)) {
                            ach.isCompleted = true;
                            Debug.Log(ach.achName + " IS COMPLETED");
                        }
                    }
                }
                break;
            case ("WEAPON"):
                foreach (Achievement ach in weaponAchievment) {
                    if (!ach.isCompleted) {
                        Debug.Log("equipped weapon is " + PlayerScript.Player.equippedWeapon.ToString() + " " + ach.achValue);
                        if (PlayerScript.Player.equippedWeapon.weaponName.ToString() == ach.achValue) {
                            ach.isCompleted = true;
                            Debug.Log(ach.achName + " IS COMPLETED");
                        }
                    }
                }
                break;
            default: break;
        }
    }
}
