using UnityEngine;

// KIERAN AND JOEL

[CreateAssetMenu(fileName = "New Character", menuName = "Assets/New Character")]
public class Character : ScriptableObject
{
    public int characterId;
    public string characterName;
    public float maxHealth;
    public float speed;
    public string spritePath; 
}
