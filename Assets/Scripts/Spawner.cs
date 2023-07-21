using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// KIERAN AND JOEL

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] float spawnerOverlapRadius;
    private EnemySpawn[] allSpawners;

    public static int currentEnemies;

    private void Start()
    {
        currentEnemies = 0;

        allSpawners = AssetManager.Assets.allEnemySpawns.ToArray();
    }

    private void Update()
    {
        if (currentEnemies < ModifierController.Modifier.totalEnemies) SpawnEnemy();

    }

    public void SpawnEnemy()
    {
        string[] selectedSpawnLocation = allSpawners[Random.Range(0, allSpawners.Length)].spawnLocation.Split('#');
        Vector3 spawnPos = new Vector2(float.Parse(selectedSpawnLocation[0]), float.Parse(selectedSpawnLocation[1]));
        Collider2D[] colliders = Physics2D.OverlapCircleAll(spawnPos, spawnerOverlapRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.CompareTag("Enemy") || collider.gameObject.CompareTag("Player"))
            {
                return;
            }
        }
        Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        currentEnemies++;
    }

}
