using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] float spawnerOverlapRadius;
    private EnemySpawn[] allSpawners;

    private int totalEnemies;
    public static int currentEnemies;

    private void Start()
    {
        this.totalEnemies = 10;
        currentEnemies = 0;

        allSpawners = AssetManager.Assets.allEnemySpawns.ToArray();
    }

    private void Update()
    {
        if (currentEnemies < totalEnemies) SpawnEnemy();

    }

    public void SpawnEnemy()
    {
        string[] selectedSpawnLocation = allSpawners[Random.Range(0, allSpawners.Length)].spawnLocation.Split('#');
        Vector3 spawnPos = new Vector2(float.Parse(selectedSpawnLocation[0]), float.Parse(selectedSpawnLocation[1]));
        Collider2D[] colliders = Physics2D.OverlapCircleAll(spawnPos, spawnerOverlapRadius);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.tag == "Enemy" || collider.gameObject.tag == "Player")
            {
                return;
            }
        }
        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
        currentEnemies++;
    }

}
