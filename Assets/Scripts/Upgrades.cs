using UnityEngine;

[CreateAssetMenu(fileName = "New Upgrade", menuName = "Assets/New Upgrade")]
public class Upgrades : ScriptableObject
{
    public int upgradeId;
    public string upgradeType;
    public int upgradeLevel;
    public float upgradeModifier;
    public int upgradeCost;
}
