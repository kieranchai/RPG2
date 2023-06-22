using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Assets/New Weapon")]
public class Weapon : ScriptableObject
{
    public int weaponId;
    public string weaponName;
    public int attackPower;
    public string spritePath;
    public string weaponType;
    public float cooldown;
    public float weaponRange;
    public int ammoCount;
    public int cost;
}
