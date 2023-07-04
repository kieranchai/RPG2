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

    private string formattedDialogueText;

    private void Start()
    {
        allDialogue = Resources.LoadAll<Dialogue>("ScriptableObjects/Dialogue");
        Array.Sort(allDialogue, (a, b) => a.dialogueId - b.dialogueId);
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
        Dialogue[] introDialogues = Array.FindAll(allDialogue, d => d.dialogueType == "INTRO");
        Dialogue introDialogue = introDialogues[Random.Range(0, introDialogues.Length)];

        this.portraitImage.sprite = Resources.Load<Sprite>(introDialogue.portraitSpritePath);
        this.portraitName.text = introDialogue.speakerName;
        this.action1Name.text = introDialogue.action1Name;
        this.action2Name.text = introDialogue.action2Name;

        this.formattedDialogueText = introDialogue.dialogueText;
        this.formattedDialogueText = this.formattedDialogueText.Replace("QUEST_TYPE", "<color=#90ee90>" + quest.questType + "</color>");
        if (quest.questAmount.Contains("#"))
        {
            this.formattedDialogueText = this.formattedDialogueText.Replace("QUEST_AMOUNT", " THIS ");
        }
        else
        {
            this.formattedDialogueText = this.formattedDialogueText.Replace("QUEST_AMOUNT", "<color=#90ee90> " + quest.questAmount + " </color>");
        }
        this.formattedDialogueText = this.formattedDialogueText.Replace("QUEST_OBJECT", "<color=#90ee90>" + quest.questObject + "</color>");
        this.formattedDialogueText = this.formattedDialogueText.Replace("$QUEST_REWARD", "<color=#90ee90>$" + quest.cashReward.ToString() + "</color>");

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
                //close dialogue, dialogueId most likely 0 so don't run code below
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
        if (currentDialogue.portraitSpritePath == "Sprites/Player")
        {
            currentDialogue.portraitSpritePath = currentDialogue.portraitSpritePath.Replace("Player", GameControllerScript.GameController.selectedCharacter.characterName);
            currentDialogue.speakerName = currentDialogue.speakerName.Replace("Player Name", GameControllerScript.GameController.selectedCharacter.characterName);
        }
        this.dialogueText.text = currentDialogue.dialogueText;
        this.portraitImage.sprite = Resources.Load<Sprite>(currentDialogue.portraitSpritePath);
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

        Dialogue[] thanksDialogues = Array.FindAll(allDialogue, d => d.dialogueType == "THANKS");
        Dialogue thanksDialogue = thanksDialogues[Random.Range(0, thanksDialogues.Length)];

        this.portraitImage.sprite = Resources.Load<Sprite>(thanksDialogue.portraitSpritePath);
        this.portraitName.text = thanksDialogue.speakerName;
        this.action1Name.text = thanksDialogue.action1Name;
        this.action2Name.text = thanksDialogue.action2Name;
        this.dialogueText.text = thanksDialogue.dialogueText;

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

        Dialogue[] introDialogues = Array.FindAll(allDialogue, d => d.dialogueType == "TUTORIAL");
        Dialogue introDialogue = introDialogues[0];

        this.portraitImage.sprite = Resources.Load<Sprite>(introDialogue.portraitSpritePath);
        this.portraitName.text = introDialogue.speakerName;
        this.action1Name.text = introDialogue.action1Name;
        this.action2Name.text = introDialogue.action2Name;
        if (introDialogue.dialogueText.Contains("CHARACTER_NAME")) introDialogue.dialogueText = introDialogue.dialogueText.Replace("CHARACTER_NAME", PlayerScript.Player.characterName);
        this.dialogueText.text = introDialogue.dialogueText;

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
}
