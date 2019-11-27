using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{

    public Transform enemyPrefab;
    public Transform spawnPoint;
    public Text waveCountdownText;
    public float timeBetweenWaves = 5f;
    public float timeBetweenEnemies;
    
    private float countdown = 2f;
    private int waveIndex = 0;

    void Update ()
    {
        if (countdown <= 0f)
        {
            // Call subprocess
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
        }

        countdown -= Time.deltaTime;

        waveCountdownText.text = Mathf.Round(countdown).ToString();
    }

    IEnumerator SpawnWave ()
    {
        waveIndex++;

        for (int i = 0; i < waveIndex; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(timeBetweenEnemies);
        }
    }

    void SpawnEnemy ()
    {
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}
