using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Assets/New Enemy")]
public class Enemy : ScriptableObject
{
    public int enemyId;
    public string enemyName;
    public float maxHealth;
    public float speed;
    public string spritePath;
    public string equippedWeaponName;
    public int xpDrop;
    public int cashDrop;
}
