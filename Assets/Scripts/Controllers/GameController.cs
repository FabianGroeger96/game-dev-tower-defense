using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public enum GameState
    {
        Init,
        Running,
        GameOver,
        Finished
    };

    public enum InputMode
    {
        Play,
        Place
    }


    [SerializeField] private Tower[] _towers;

    public GameState gameState = GameState.Init;
    public Text gameOverText;
    public int initialLifeCount = 100;
    public int initialMoneyCount = 1000;

    private InputMode _inputMode;
    private int _currentPlacingTower;

    [SerializeField] private int _moneyCount;
    [SerializeField] private int _lifeCount;

    private PlacementController _placementController;
    private WaveSpawner _waveSpawner;
    private UIController _uiController;

    public float timePlayed = 0f;

    private float minutes;
    private float seconds;

    // variables for spawning enemies
    private float _countdownWave = 2;
    private int _waveIndex = 0;

    public Wave[] waves;

    public float timeBetweenWaves = 5;
    public static int enemiesAlive = 0;

    void Start()
    {
        //Get controllers
        _placementController = GetComponent<PlacementController>();
        _waveSpawner = GetComponent<WaveSpawner>();
        _uiController = GetComponent<UIController>();

        // initial game state
        gameState = GameState.Running;
        _inputMode = InputMode.Play;
        _currentPlacingTower = -1;

        // initial life count
        _lifeCount = initialLifeCount;
        _moneyCount = initialMoneyCount;

        // initial UI Elements
        _uiController.setLifeCountText(_lifeCount.ToString());
        _uiController.setWaveCountText(_countdownWave.ToString());
        _uiController.setMoneyCountText(_moneyCount.ToString());
    }

    void Update()
    {
        switch (gameState)
        {
            case GameState.Init:
                break;

            case GameState.Running:
                gameOverText.enabled = false;
                _uiController.lifeCountText.enabled = true;

                if (enemiesAlive == 0)
                {
                    if (_countdownWave <= 0f)
                    {
                        // Call subprocess
                        Wave wave = waves[_waveIndex];
                        StartCoroutine(_waveSpawner.SpawnWave(wave));

                        _waveIndex++;
                        _countdownWave = timeBetweenWaves;

                        if (_waveIndex == waves.Length)
                        {
                            gameState = GameState.Finished;
                        }
                    }

                    _countdownWave -= Time.deltaTime;

                    updateUI();
                }

                break;
            case GameState.GameOver:
                gameOverText.enabled = true;
                _uiController.lifeCountText.enabled = false;
                
                enabled = false;
                break;
            case GameState.Finished:
                gameOverText.text = "Finished";
                gameOverText.enabled = true;
                _uiController.lifeCountText.enabled = false;

                enabled = false;
                break;
        }
    }

    private void updateUI()
    {
        // update UI elements
        _uiController.setWaveCountText(Mathf.Round(_countdownWave).ToString());
        _uiController.setLifeCountText(_lifeCount.ToString());
        _uiController.setMoneyCountText(_moneyCount.ToString());

        timePlayed += Time.deltaTime;
        minutes = Mathf.Floor(timePlayed / 60);
        seconds = timePlayed % 60;
        if (seconds > 59) seconds = 59;
        if (minutes < 0)
        {
            minutes = 0;
            seconds = 0;
        }

        _uiController.setTimeCountText(string.Format("{0:0}:{1:00}", minutes, seconds));
    }

    public void ExitPlacementMode()
    {
        _inputMode = InputMode.Play;
        _currentPlacingTower = -1;
        _placementController.SetPlacementModeInactive();
    }

    public void SetToPlacementMode(int towerNumber)
    {
        if (_inputMode == InputMode.Play)
        {
            _inputMode = InputMode.Place;
            _currentPlacingTower = towerNumber;
            Tower _tower = _towers[towerNumber - 1];
            if (_tower.costs <= _moneyCount)
            {
                _placementController.SetPlacementModeActive(_tower);
            }
        }
        else
        {
            if (_currentPlacingTower != towerNumber)
            {
                ExitPlacementMode();
                SetToPlacementMode(towerNumber);
            }
            else
            {
                ExitPlacementMode();
            }
        }
    }

    public void RemoveLifeCount(int lives)
    {
        _lifeCount -= lives;
        if (_lifeCount <= 0)
        {
            Debug.Log("Game Over");
            gameState = GameState.GameOver;
        }
    }

    public void TowerPlaced(int costs)
    {
        _moneyCount -= _towers[_currentPlacingTower - 1].costs;
    }

    public void RegisterKill(int earning)
    {
        _moneyCount += earning;
    }
}