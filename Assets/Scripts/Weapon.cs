using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Assets/New Weapon")]
public class Weapon : ScriptableObject
{
    public int weaponId;
    public string weaponName;
    public int attackPower;
}
