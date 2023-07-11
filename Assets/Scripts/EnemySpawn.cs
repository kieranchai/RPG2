using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Spawn", menuName = "Assets/New Enemy Spawn")]
public class EnemySpawn : ScriptableObject
{
    public int enemySpawnId;
    public string spawnLocation;
}
