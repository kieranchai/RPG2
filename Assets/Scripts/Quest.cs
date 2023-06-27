using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Assets/New Quest")]
public class Quest : ScriptableObject
{
    public int questId;
    public string questType;
    public string questAmount;
    public string questObject;
    public int xpReward;
    public int cashReward;
}
