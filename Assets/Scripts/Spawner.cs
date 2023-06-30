using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Transform[] spawnLocations;
    [SerializeField] float spawnerOverlapRadius;

    private int totalEnemies;
    public static int currentEnemies;

    private void Start()
    {
        this.totalEnemies = 10;
        currentEnemies = 0;
    }

    private void Update()
    {
        if (currentEnemies < totalEnemies) SpawnEnemy();

    }

    public void SpawnEnemy()
    {
        Vector3 spawnPos = spawnLocations[Random.Range(0, spawnLocations.Length)].position;
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
