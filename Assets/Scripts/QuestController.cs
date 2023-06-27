using System;
using System.Collections;
using System.Collections.Generic;
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

    private string[] currentQuestCoords = new string[2];

    private float timer;
    private float questInterval = 10f;

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
        if (activeQuest || givenQuest) return;
        givenQuest = allQuests[Random.Range(0, allQuests.Length)];

        // Pass Quest to Dialogue
        this.dialogueController.StartDialogue(givenQuest);
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
    }

    public void RejectQuest()
    {
        this.givenQuest = null;
        this.timer = 0;
    }

    public void FinishQuest(Quest givenQuest)
    {
        this.dialogueController.QuestFinishDialogue();
        this.activeQuest = null;
        this.timer = 0;
        PopupController.Popup.UpdatePopUp("QUEST PASSED!");
        PlayerScript.Player.cash += givenQuest.cashReward;
        PlayerScript.Player.UpdateCash(givenQuest.cashReward);
        PlayerScript.Player.UpdateExperience(givenQuest.xpReward);
    }

    public void KillQuestProgress()
    {
        // Can check if halfway or not ... add new if statements
        if ((AnalyticsController.Analytics.enemiesKilled - this.killCount) / 2 == int.Parse(activeQuest.questAmount))
        {
            this.dialogueController.ContinueDialogue("", 9);
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
        if ((this.questLocation.position - PlayerScript.Player.transform.position).magnitude < 10f)
        {
            this.dialogueController.ContinueDialogue("", 9);
        }

        // Finished Quest
        if ((this.questLocation.position - PlayerScript.Player.transform.position).magnitude < 0.5f) {
            FinishQuest(activeQuest);
            PlayerScript.Player.transform.Find("Quest Target Indicator").gameObject.SetActive(false);
            this.gotoLocation.SetActive(false);
        }
    }
}
