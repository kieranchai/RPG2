using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

// KIERAN AND JOEL

public class AnalyticsController : MonoBehaviour
{
    public static AnalyticsController Analytics { get; private set; }

    public int enemiesKilled = 0;
    public float damageTaken = 0;
    public float damageDealt = 0;
    public int timePlayed = 0;
    public int experienceGained = 0;
    public int questCompleted = 0;
    public Achievement[] allAchievements;
    public Achievement[] questAchievements;
    public Achievement[] killAchievements;
    public Achievement[] timeAchievements;
    public Achievement[] weaponAchievements;
    public Achievement[] damageDealtAchievements;
    public Achievement[] damageTakenAchievements;

    private float timer;

    [SerializeField] private GameObject statisticsDisplay;
    [SerializeField] private GameObject notifPanel;
    [SerializeField] private GameObject notifPrefab;

    private void Awake()
    {
        if (Analytics != null && Analytics != this)
        {
            Destroy(gameObject);
            return;
        }
        Analytics = this;

        allAchievements = AssetManager.Assets.allAchievements.ToArray();
        Array.Sort(allAchievements, (a, b) => a.achId - b.achId);

        foreach (Achievement ach in allAchievements)
        {
            ach.isCompleted = false;
        }

        questAchievements = Array.FindAll(allAchievements, element => element.achType == "QUEST");
        Array.Sort(questAchievements, (a, b) => a.achId - b.achId);

        killAchievements = Array.FindAll(allAchievements, element => element.achType == "KILL");
        Array.Sort(killAchievements, (a, b) => a.achId - b.achId);

        timeAchievements = Array.FindAll(allAchievements, element => element.achType == "TIMEPLAYED");
        Array.Sort(timeAchievements, (a, b) => a.achId - b.achId);

        weaponAchievements = Array.FindAll(allAchievements, element => element.achType == "WEAPON");
        Array.Sort(weaponAchievements, (a, b) => a.achId - b.achId);

        damageDealtAchievements = Array.FindAll(allAchievements, element => element.achType == "DAMAGEDEALT");
        Array.Sort(damageDealtAchievements, (a, b) => a.achId - b.achId);

        damageTakenAchievements = Array.FindAll(allAchievements, element => element.achType == "DAMAGETAKE");
        Array.Sort(damageTakenAchievements, (a, b) => a.achId - b.achId);
    }

    private void Update()
    {
        if (GameControllerScript.GameController.isAlive) timer += Time.deltaTime;
        timePlayed = (int)timer;
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

    public void PopUpNotif(Achievement ach)
    {
        AudioManager.instance.PlaySFX("Achievement");
        GameObject notif = Instantiate(notifPrefab, notifPanel.transform);
        notif.GetComponent<Notification>().Initialise(ach.achName, ach.achDesc, ach.achValue, ach.achType);
    }

    public void CheckAchievements(string type)
    {
        switch (type)
        {
            case "KILL":
                foreach (Achievement ach in killAchievements)
                {
                    if (ach.isCompleted) continue;
                    if (enemiesKilled >= int.Parse(ach.achValue))
                    {
                        ach.isCompleted = true;
                        PopUpNotif(ach);
                    }
                }
                break;
            case "QUEST":
                foreach (Achievement ach in questAchievements)
                {
                    if (ach.isCompleted) continue;
                    if (questCompleted >= int.Parse(ach.achValue))
                    {
                        ach.isCompleted = true;
                        PopUpNotif(ach);
                    }
                }
                break;
            case "TIME":
                foreach (Achievement ach in timeAchievements)
                {
                    if (ach.isCompleted) continue;
                    if (this.timePlayed >= int.Parse(ach.achValue))
                    {
                        ach.isCompleted = true;
                        PopUpNotif(ach);
                    }
                }
                break;
            case "WEAPON":
                foreach (Achievement ach in weaponAchievements)
                {
                    if (ach.isCompleted) continue;
                    if (PlayerScript.Player.equippedWeapon.weaponName.ToString() == ach.achValue)
                    {
                        ach.isCompleted = true;
                        PopUpNotif(ach);
                    }
                }
                break;
            case "DAMAGEDEALT":
                foreach (Achievement ach in damageDealtAchievements)
                {
                    if (ach.isCompleted) continue;
                    if (this.damageDealt >= float.Parse(ach.achValue))
                    {
                        ach.isCompleted = true;
                        PopUpNotif(ach);
                    }
                }
                break;
            case "DAMAGETAKEN":
                foreach (Achievement ach in damageTakenAchievements)
                {
                    if (ach.isCompleted) continue;
                    if (this.damageTaken >= float.Parse(ach.achValue))
                    {
                        ach.isCompleted = true;
                        PopUpNotif(ach);
                    }
                }
                break;
            default:
                break;
        }
    }
}
