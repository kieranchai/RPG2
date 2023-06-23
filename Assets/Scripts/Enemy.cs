using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "Assets/New Enemy")]
public class Enemy : ScriptableObject
{
    public int enemyId;
    public string enemyName;
    public int maxHealth;
    public float speed;
    public string spritePath;
    public string equippedWeaponName;
}
