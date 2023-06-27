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

        InvokeRepeating("GiveQuest", 5f, 30f);
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
    }

    public void GiveQuest()
    {
        if (activeQuest) return;
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
    }

    public void RejectQuest()
    {
        this.givenQuest = null;
    }

    public void FinishQuest(Quest givenQuest)
    {
        // Play dialogue
        this.dialogueController.QuestFinishDialogue();

        this.activeQuest = null;

        PlayerScript.Player.cash += givenQuest.cashReward;
        PlayerScript.Player.UpdateCash(givenQuest.cashReward);
        PlayerScript.Player.UpdateExperience(givenQuest.xpReward);
    }

    public void KillQuestProgress()
    {
        // Can check if halfway or not ... add new if statements

        // Finished Quest
        if (AnalyticsController.Analytics.enemiesKilled == (int.Parse(activeQuest.questAmount) + this.killCount))
        {
            FinishQuest(activeQuest);
        }
    }

    public void GotoQuestProgress()
    {
        // Can check if halfway or not ... add new if statements

        // Finished Quest
        if ((this.questLocation.position - PlayerScript.Player.transform.position).magnitude < 0.5f) {
            FinishQuest(activeQuest);
            PlayerScript.Player.transform.Find("Quest Target Indicator").gameObject.SetActive(false);
            this.gotoLocation.SetActive(false);
        }
    }
}
