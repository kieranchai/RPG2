using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

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
    }

    public void StartDialogue(Quest quest)
    {
        dialoguePanel.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Button>().onClick.RemoveAllListeners();
        dialoguePanel.transform.GetChild(1).GetChild(1).GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
        this.portraitImage.sprite = Resources.Load<Sprite>(allDialogue[0].portraitSpritePath);
        this.portraitName.text = allDialogue[0].speakerName;
        this.action1Name.text = allDialogue[0].action1Name;
        this.action2Name.text = allDialogue[0].action2Name;

        this.formattedDialogueText = allDialogue[0].dialogueText;
        this.formattedDialogueText = this.formattedDialogueText.Replace("QUEST_TYPE", "<color=#387182>" + quest.questType + "</color>");
        if (quest.questAmount == 0)
        {
            this.formattedDialogueText = this.formattedDialogueText.Replace("QUEST_AMOUNT", " ");
        }
        else
        {
            this.formattedDialogueText = this.formattedDialogueText.Replace("QUEST_AMOUNT", "<color=#387182> " + quest.questAmount + " </color>");
        }
        this.formattedDialogueText = this.formattedDialogueText.Replace("QUEST_OBJECT", "<color=#387182>" + quest.questObject + "</color>");
        this.formattedDialogueText = this.formattedDialogueText.Replace("$QUEST_REWARD", "<color=#387182>$" + quest.questReward.ToString() + "</color>");

        this.dialogueText.text = this.formattedDialogueText;

        dialoguePanel.transform.GetChild(1).GetChild(1).GetChild(0).GetComponent<Button>().onClick.AddListener(() => ContinueDialogue(allDialogue[0].action1Name, allDialogue[0].action1DialogueId));
        dialoguePanel.transform.GetChild(1).GetChild(1).GetChild(1).GetComponent<Button>().onClick.AddListener(() => ContinueDialogue(allDialogue[0].action2Name, allDialogue[0].action2DialogueId));

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
    }
}
