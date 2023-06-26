using UnityEngine;

[CreateAssetMenu(fileName = "New Dialogue", menuName = "Assets/New Dialogue")]
public class Dialogue : ScriptableObject
{
    public int dialogueId;
    public string portraitSpritePath;
    public string speakerName;
    public string dialogueText;
    public string action1Name;
    public string action2Name;
    public int action1DialogueId;
    public int action2DialogueId;
}
