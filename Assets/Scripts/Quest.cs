using UnityEngine;

[CreateAssetMenu(fileName = "New Quest", menuName = "Assets/New Quest")]
public class Quest : ScriptableObject
{
    public int questId;
    public string questType;
    public int questAmount;
    public string questObject;
    public int questReward;
}
