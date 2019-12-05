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
        GameOver
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

    // variables for spawning enemies
    private float _countdownWave = 2;
    private int _waveIndex = 0;

    public float timeBetweenWaves = 5;
    public float timeBetweenEnemies = 2;

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
        _uiController.lifeCountText.text = _lifeCount.ToString();
        _uiController.waveCountdownText.text = _countdownWave.ToString();
    }

    void Update()
    {
        switch (gameState)
        {
            case GameState.Running:
                gameOverText.enabled = false;
                _uiController.lifeCountText.enabled = true;

                // update life count
                _uiController.lifeCountText.text = _lifeCount.ToString();

                if (_countdownWave <= 0f)
                {
                    // Call subprocess
                    StartCoroutine(_waveSpawner.SpawnWave(_waveIndex, timeBetweenEnemies));
                    _waveIndex++;
                    _countdownWave = timeBetweenWaves;
                }

                _countdownWave -= Time.deltaTime;
                _uiController.waveCountdownText.text = Mathf.Round(_countdownWave).ToString();

                break;
            case GameState.GameOver:
                gameOverText.enabled = true;
                _uiController.lifeCountText.enabled = false;
                break;
        }
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