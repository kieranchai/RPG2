using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "Assets/New Character")]
public class Character : ScriptableObject
{
    public int characterId;
    public string characterName;
    public int maxHealth;
    public float speed;
    public string spritePath; 
}
