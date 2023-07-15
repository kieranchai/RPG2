using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Random = UnityEngine.Random;

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
        IntroDialogue();
    }

    private void Update()
    {
        if (!GameControllerScript.GameController.isAlive) dialoguePanel.SetActive(false);
    }

    public void StartDialogue(Quest quest)
    {
        dialoguePanel.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
        dialoguePanel.transform.GetChild(1).GetChild(1).GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
        Dialogue introDialogue = introDialogues[Random.Range(0, introDialogues.Length)];

        this.portraitImage.sprite = AssetManager.Assets.GetSprite(introDialogue.portraitSpritePath);

        this.portraitName.text = introDialogue.speakerName;
        this.action1Name.text = introDialogue.action1Name;
        this.action2Name.text = introDialogue.action2Name;

        this.formattedDialogueText = FormatDialogueText(introDialogue.dialogueText, quest);
        this.dialogueText.text = this.formattedDialogueText;

        dialoguePanel.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Button>().onClick.AddListener(() => ContinueDialogue(introDialogue.action1Name, introDialogue.action1DialogueId));
        dialoguePanel.transform.GetChild(1).GetChild(1).GetChild(1).GetComponent<Button>().onClick.AddListener(() => ContinueDialogue(introDialogue.action2Name, introDialogue.action2DialogueId));

        dialoguePanel.SetActive(true);
        dialoguePanel.transform.GetChild(1).GetChild(1).GetChild(1).gameObject.SetActive(true);
    }

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
        if (currentDialogue.portraitSpritePath.Contains("Player"))
        {
            currentDialogue.portraitSpritePath = currentDialogue.portraitSpritePath.Replace("Player", GameControllerScript.GameController.selectedCharacter.characterName);
            currentDialogue.speakerName = currentDialogue.speakerName.Replace("Player Name", GameControllerScript.GameController.selectedCharacter.characterName);
        }

        this.formattedDialogueText = FormatDialogueText(currentDialogue.dialogueText, null);
        this.dialogueText.text = this.formattedDialogueText;
        this.portraitImage.sprite = AssetManager.Assets.GetSprite(currentDialogue.portraitSpritePath);
        this.portraitName.text = currentDialogue.speakerName;
        this.action1Name.text = currentDialogue.action1Name;

        if (currentDialogue.action2Name == "NULL")
        {
            this.action2Name.text = currentDialogue.action2Name;
            dialoguePanel.transform.GetChild(1).GetChild(1).GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            dialoguePanel.transform.GetChild(1).GetChild(1).GetChild(1).gameObject.SetActive(true);
            this.action2Name.text = currentDialogue.action2Name;
        }

        dialoguePanel.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Button>().onClick.AddListener(() => ContinueDialogue(currentDialogue.action1Name, currentDialogue.action1DialogueId));
        dialoguePanel.transform.GetChild(1).GetChild(1).GetChild(1).GetComponent<Button>().onClick.AddListener(() => ContinueDialogue(currentDialogue.action2Name, currentDialogue.action2DialogueId));

        dialoguePanel.SetActive(true);
    }

    public void QuestFinishDialogue()
    {
        dialoguePanel.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
        dialoguePanel.transform.GetChild(1).GetChild(1).GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();

        Dialogue thanksDialogue = thanksDialogues[Random.Range(0, thanksDialogues.Length)];

        this.portraitImage.sprite = AssetManager.Assets.GetSprite(thanksDialogue.portraitSpritePath);
        this.portraitName.text = thanksDialogue.speakerName;
        this.action1Name.text = thanksDialogue.action1Name;
        this.action2Name.text = thanksDialogue.action2Name;

        this.formattedDialogueText = FormatDialogueText(thanksDialogue.dialogueText, null);
        this.dialogueText.text = this.formattedDialogueText;

        if (thanksDialogue.action2Name == "NULL")
        {
            this.action2Name.text = thanksDialogue.action2Name;
            dialoguePanel.transform.GetChild(1).GetChild(1).GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            dialoguePanel.transform.GetChild(1).GetChild(1).GetChild(1).gameObject.SetActive(true);
            this.action2Name.text = thanksDialogue.action2Name;
        }

        dialoguePanel.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Button>().onClick.AddListener(() => ContinueDialogue(thanksDialogue.action1Name, thanksDialogue.action1DialogueId));
        dialoguePanel.transform.GetChild(1).GetChild(1).GetChild(1).GetComponent<Button>().onClick.AddListener(() => ContinueDialogue(thanksDialogue.action2Name, thanksDialogue.action2DialogueId));
        dialoguePanel.SetActive(true);
    }

    public void IntroDialogue()
    {
        dialoguePanel.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
        dialoguePanel.transform.GetChild(1).GetChild(1).GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();

        Dialogue introDialogue = tutorialDialogues[0];

        this.portraitImage.sprite = AssetManager.Assets.GetSprite(introDialogue.portraitSpritePath);
        this.portraitName.text = introDialogue.speakerName;
        this.action1Name.text = introDialogue.action1Name;
        this.action2Name.text = introDialogue.action2Name;

        this.formattedDialogueText = FormatDialogueText(introDialogue.dialogueText, null);
        this.dialogueText.text = this.formattedDialogueText;

        if (introDialogue.action2Name == "NULL")
        {
            this.action2Name.text = introDialogue.action2Name;
            dialoguePanel.transform.GetChild(1).GetChild(1).GetChild(1).gameObject.SetActive(false);
        }
        else
        {
            dialoguePanel.transform.GetChild(1).GetChild(1).GetChild(1).gameObject.SetActive(true);
            this.action2Name.text = introDialogue.action2Name;
        }

        dialoguePanel.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Button>().onClick.AddListener(() => ContinueDialogue(introDialogue.action1Name, introDialogue.action1DialogueId));
        dialoguePanel.transform.GetChild(1).GetChild(1).GetChild(1).GetComponent<Button>().onClick.AddListener(() => ContinueDialogue(introDialogue.action2Name, introDialogue.action2DialogueId));
        dialoguePanel.SetActive(true);
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
}
