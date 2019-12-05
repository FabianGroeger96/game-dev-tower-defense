using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{
    public Transform spawnPoint;
    public Transform standardEnemyPrefab;
    public Transform tankEnemyPrefab;

    public IEnumerator SpawnWave(int waveIndex, float timeBetweenEnemies)
    {
        Transform enemy;
        // Tank wave
        if (waveIndex % 5 == 0)
        {
            waveIndex = waveIndex / 2;
            enemy = tankEnemyPrefab;
        }
        else
        {
            enemy = standardEnemyPrefab;
        }

        for (int i = 0; i < waveIndex; i++)
        {
            SpawnEnemy(enemy);
            yield return new WaitForSeconds(timeBetweenEnemies);
        }
    }

    void SpawnEnemy(Transform enemy)
    {
        Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
    }
}