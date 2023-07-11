using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class QuestController : MonoBehaviour
{
    public static QuestController Quest { get; private set; }

    public Quest activeQuest;
    public Quest givenQuest;

    private Quest[] allQuests;

    private int killCount;
    [SerializeField] private GameObject gotoLocation;
    public Transform questLocation;

    [SerializeField] private DialogueController dialogueController;
    [SerializeField] private GameObject questLog;

    private string[] currentQuestCoords = new string[2];

    private float timer;
    private float questInterval = 30f;

    private bool hasMentionedHalf = false;

    private void Awake()
    {
        if (Quest != null && Quest != this)
        {
            Destroy(gameObject);
            return;
        }
        Quest = this;
    }

    private void Start()
    {
        allQuests = Resources.LoadAll<Quest>("ScriptableObjects/Quests");
        Array.Sort(allQuests, (a, b) => a.questId - b.questId);
    }

    private void Update()
    {
        if (activeQuest != null)
        {
            UpdateQuestLog();
            switch (activeQuest.questType)
            {
                case "KILL":
                    KillQuestProgress();
                    break;
                case "GO TO":
                    GotoQuestProgress();
                    break;
                default:
                    break;
            }
        }

        this.timer += Time.deltaTime;
        if(this.timer >= this.questInterval)
        {
            GiveQuest();
            this.timer = 0;
        }
        
    }

    public void GiveQuest()
    {
        if (activeQuest || givenQuest || !GameControllerScript.GameController.hasPlayedTutorial) return;
        givenQuest = allQuests[Random.Range(0, allQuests.Length)];

        AudioManager.instance.PlaySFX("Quest Ringing");
        StartCoroutine("QuestwaitForSeconds");
        // Pass Quest to Dialogue
    }

    IEnumerator QuestwaitForSeconds() {
        yield return new WaitForSeconds(2f);
        AudioManager.instance.sfxSource.Stop();
        this.dialogueController.StartDialogue(givenQuest);
        yield break;
    }
    public void AcceptQuest(Quest givenQuest)
    {
        if (givenQuest.questType == "KILL")
        {
            this.killCount = AnalyticsController.Analytics.enemiesKilled;
        }
        else if (givenQuest.questType == "GO TO")
        {
            currentQuestCoords = givenQuest.questAmount.Split('#');
            gotoLocation.transform.localPosition = new Vector2(float.Parse(currentQuestCoords[0]), float.Parse(currentQuestCoords[1]));
            this.questLocation = gotoLocation.transform;
            this.gotoLocation.SetActive(true);
            PlayerScript.Player.transform.Find("Quest Target Indicator").GetComponent<TargetIndicator>().target = this.questLocation;
            PlayerScript.Player.transform.Find("Quest Target Indicator").gameObject.SetActive(true);
        }

        this.activeQuest = givenQuest;
        this.givenQuest = null;
        PopupController.Popup.UpdatePopUp("QUEST START!");
        questLog.SetActive(true);
    }

    public void RejectQuest()
    {
        this.givenQuest = null;
        this.timer = 0;
    }

    public void UpdateQuestLog()
    {
        if (!this.activeQuest) return;

        switch(this.activeQuest.questType) {
            case "KILL":
                questLog.transform.Find("Quest Desc").GetComponent<TMP_Text>().text = $"{this.activeQuest.questType} {this.activeQuest.questAmount} {this.activeQuest.questObject}";
                questLog.transform.Find("Tracker").GetComponent<TMP_Text>().text = $"{AnalyticsController.Analytics.enemiesKilled - this.killCount}/{this.activeQuest.questAmount}";
                break;
            case "GO TO":
                questLog.transform.Find("Quest Desc").GetComponent<TMP_Text>().text = $"{this.activeQuest.questType} THE {this.activeQuest.questObject}";
                questLog.transform.Find("Tracker").GetComponent<TMP_Text>().text = $"FOLLOW THE YELLOW MARKER";
                break;
            default:
                break;
        }
    }

    public void FinishQuest(Quest givenQuest)
    {
        this.dialogueController.QuestFinishDialogue();
        questLog.SetActive(false);
        this.activeQuest = null;
        this.timer = 0;
        this.hasMentionedHalf = false;
        this.questLocation = null;
        PopupController.Popup.UpdatePopUp("QUEST PASSED!");
        PlayerScript.Player.cash += givenQuest.cashReward;
        PlayerScript.Player.UpdateCash(givenQuest.cashReward);
        PlayerScript.Player.UpdateExperience(givenQuest.xpReward);
        AnalyticsController.Analytics.questCompleted++;
        AnalyticsController.Analytics.CheckAchievements("QUEST");
    }

    public void KillQuestProgress()
    {
        // Can check if halfway or not ... add new if statements
        if ((AnalyticsController.Analytics.enemiesKilled - this.killCount) / 2 == int.Parse(activeQuest.questAmount) && !this.hasMentionedHalf)
        {
            this.dialogueController.ContinueDialogue("", 9);
            this.hasMentionedHalf = true;
        }
        
        // Finished Quest
        if (AnalyticsController.Analytics.enemiesKilled >= (int.Parse(activeQuest.questAmount) + this.killCount))
        {
            FinishQuest(activeQuest);
        }
    }

    public void GotoQuestProgress()
    {
        // Can check if halfway or not ... add new if statements
        if ((this.questLocation.position - PlayerScript.Player.transform.position).magnitude < 10f && !this.hasMentionedHalf)
        {
            this.dialogueController.ContinueDialogue("", 9);
            this.hasMentionedHalf = true;
        }

        // Finished Quest
        if ((this.questLocation.position - PlayerScript.Player.transform.position).magnitude < 0.5f) {
            FinishQuest(activeQuest);
            PlayerScript.Player.transform.Find("Quest Target Indicator").gameObject.SetActive(false);
            this.gotoLocation.SetActive(false);
        }
    }
}
