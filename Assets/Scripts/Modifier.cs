using UnityEngine;

// KIERAN AND JOEL

[CreateAssetMenu(fileName = "New Modifier", menuName = "Assets/New Modifier")]
public class Modifier : ScriptableObject
{
    public int modId;
    public int xpNeeded;
    public float speedMod;
    public float apMod;
    public int totalEnemies;
}
