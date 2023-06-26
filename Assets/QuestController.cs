using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestController : MonoBehaviour
{
    public static QuestController Quest { get; private set; }

    public Quest activeQuest;
    public Quest givenQuest;

    private Quest[] allQuests;

    private int killCount;
    [SerializeField] private GameObject[] gotoLocations;
    public Transform questLocation;
    public GameObject randomLocation;

    [SerializeField] private DialogueController dialogueController;

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
        InvokeRepeating("GiveQuest", 5f, 10f);
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
            this.randomLocation = gotoLocations[Random.Range(0, gotoLocations.Length)];
            this.questLocation = randomLocation.transform;
            this.randomLocation.SetActive(true);
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
        Debug.Log("Finished Quest");

        this.activeQuest = null;
        PlayerScript.Player.cash += givenQuest.questReward;
        PlayerScript.Player.UpdateCash(givenQuest.questReward);
    }

    public void KillQuestProgress()
    {
        // Can check if halfway or not ... add new if statements

        // Finished Quest
        if (AnalyticsController.Analytics.enemiesKilled - this.killCount == activeQuest.questAmount)
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
            this.randomLocation.SetActive(false);
        }
    }
}
