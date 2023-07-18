using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Random = UnityEngine.Random;

// KIERAN

public class DialogueController : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private TMP_Text portraitName;
    [SerializeField] private Image portraitImage;
    [SerializeField] private TMP_Text action1Name;
    [SerializeField] private TMP_Text action2Name;

    private Dialogue[] allDialogue;
    private Dialogue[] introDialogues;
    private Dialogue[] thanksDialogues;
    private Dialogue[] tutorialDialogues;

    private string formattedDialogueText;

    private void Start()
    {
        allDialogue = AssetManager.Assets.allDialogue.ToArray();
        Array.Sort(allDialogue, (a, b) => a.dialogueId - b.dialogueId);

        introDialogues = Array.FindAll(allDialogue, d => d.dialogueType == "INTRO");
        thanksDialogues = Array.FindAll(allDialogue, d => d.dialogueType == "THANKS");
        tutorialDialogues = Array.FindAll(allDialogue, d => d.dialogueType == "TUTORIAL");

        GameControllerScript.GameController.hasPlayedTutorial = false;
        StartDialogue("TUTORIAL", null);
    }

    private void Update()
    {
        if (!GameControllerScript.GameController.isAlive) dialoguePanel.SetActive(false);
    }

    #nullable enable
    public void StartDialogue(string dialogueType, Quest? quest)
    {
        dialoguePanel.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
        dialoguePanel.transform.GetChild(1).GetChild(1).GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();

        Dialogue dialogue = tutorialDialogues[0]; //placeholder dialogue
        switch (dialogueType)
        {
            case "INTRO":
                dialogue = introDialogues[Random.Range(0, introDialogues.Length)];
                break;
            case "THANKS":
                dialogue = thanksDialogues[Random.Range(0, thanksDialogues.Length)];
                break;
            case "TUTORIAL":
                dialogue = tutorialDialogues[0];
                break;
            default:
                break;
        }

        this.formattedDialogueText = FormatDialogueText(dialogue.dialogueText, quest);
        this.dialogueText.text = this.formattedDialogueText;
        FormatDialoguePanel(dialogue);

        dialoguePanel.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Button>().onClick.AddListener(() => ContinueDialogue(dialogue.action1Name, dialogue.action1DialogueId));
        dialoguePanel.transform.GetChild(1).GetChild(1).GetChild(1).GetComponent<Button>().onClick.AddListener(() => ContinueDialogue(dialogue.action2Name, dialogue.action2DialogueId));
        dialoguePanel.SetActive(true);
    }
    #nullable disable

    public void ContinueDialogue(string actionName, int actionDialogueId)
    {
        dialoguePanel.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
        dialoguePanel.transform.GetChild(1).GetChild(1).GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();

        switch (actionName)
        {
            case "YES":
                QuestController.Quest.AcceptQuest(QuestController.Quest.givenQuest);
                break;
            case "NO":
                QuestController.Quest.RejectQuest();
                break;
            case "CLOSE":
                GameControllerScript.GameController.canAttack = true;
                dialoguePanel.SetActive(false);
                return;
            case "DONE":
                GameControllerScript.GameController.canAttack = true;
                GameControllerScript.GameController.hasPlayedTutorial = true;
                dialoguePanel.SetActive(false);
                return;
            default:
                break;
        }

        Dialogue currentDialogue = Array.Find(allDialogue, dialogue => dialogue.dialogueId == actionDialogueId);
        this.formattedDialogueText = FormatDialogueText(currentDialogue.dialogueText, null);
        this.dialogueText.text = this.formattedDialogueText;
        FormatDialoguePanel(currentDialogue);


        dialoguePanel.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Button>().onClick.AddListener(() => ContinueDialogue(currentDialogue.action1Name, currentDialogue.action1DialogueId));
        dialoguePanel.transform.GetChild(1).GetChild(1).GetChild(1).GetComponent<Button>().onClick.AddListener(() => ContinueDialogue(currentDialogue.action2Name, currentDialogue.action2DialogueId));
        dialoguePanel.SetActive(true);
    }

    public void FormatDialoguePanel(Dialogue dialogue)
    {
        if (dialogue.portraitSpritePath.Contains("Player"))
        {
            dialogue.portraitSpritePath = dialogue.portraitSpritePath.Replace("Player", GameControllerScript.GameController.selectedCharacter.characterName);
            dialogue.speakerName = dialogue.speakerName.Replace("Player Name", GameControllerScript.GameController.selectedCharacter.characterName);
        }

        this.portraitImage.sprite = AssetManager.Assets.GetSprite(dialogue.portraitSpritePath);
        this.portraitName.text = dialogue.speakerName;
        this.action1Name.text = dialogue.action1Name;
        this.action2Name.text = dialogue.action2Name;

        if (dialogue.action2Name == "NULL")
        {
            this.action2Name.text = dialogue.action2Name;
            dialoguePanel.transform.GetChild(1).GetChild(1).GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            dialoguePanel.transform.GetChild(1).GetChild(1).GetChild(1).gameObject.SetActive(true);
            this.action2Name.text = dialogue.action2Name;
        }
    }

    #nullable enable
    public string FormatDialogueText(string text, Quest? quest)
    {
        if (quest)
        {
            #pragma warning disable CS8602 // Dereference of a possibly null reference.
            text = text.Replace("QUEST_TYPE", quest.questType);
            #pragma warning restore CS8602 // Dereference of a possibly null reference.
            if (quest.questAmount.Contains("#"))
            {
                text = text.Replace("QUEST_AMOUNT", "THIS");
            }
            else
            {
                text = text.Replace("QUEST_AMOUNT", quest.questAmount);
            }
            text = text.Replace("QUEST_OBJECT", quest.questObject);
            text = text.Replace("QUEST_REWARD", quest.cashReward.ToString());
        }


        if (text.Contains("CHARACTER_NAME")) text = text.Replace("CHARACTER_NAME", GameControllerScript.GameController.selectedCharacter.characterName);
        if (text.Contains("#")) text = text.Replace("#", ", ");
        if (text.Contains("[COLOR]")) text = text.Replace("[COLOR]", "<color=#90ee90>");
        if (text.Contains("[!COLOR]")) text = text.Replace("[!COLOR]", "</color>");
        if (text.Contains("[GREEN]")) text = text.Replace("[GREEN]", "<color=#2fd454>");
        if (text.Contains("[!GREEN]")) text = text.Replace("[!GREEN]", "</color>");
        if (text.Contains("[RED]")) text = text.Replace("[RED]", "<color=#d62000>");
        if (text.Contains("[!RED]")) text = text.Replace("[!RED]", "</color>");

        return text;
    }
    #nullable disable
}
