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

    private float _countdown = 2f;
    private int _waveIndex = 0;
    private GameController _gameController;

    private void Start()
    {
        _gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }

    void Update()
    {
        if (_gameController.gameState == GameController.GameState.Running)
        {
            if (_countdown <= 0f)
            {
                // Call subprocess
                StartCoroutine(SpawnWave());
                _countdown = timeBetweenWaves;
            }

            _countdown -= Time.deltaTime;

            waveCountdownText.text = Mathf.Round(_countdown).ToString();
        }
        else
        {
            waveCountdownText.enabled = false;
            waveCountdownText.text = "";
        }
    }

    IEnumerator SpawnWave()
    {
        _waveIndex++;

        for (int i = 0; i < _waveIndex; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(timeBetweenEnemies);
        }
    }

    void SpawnEnemy()
    {
        Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
    }
}