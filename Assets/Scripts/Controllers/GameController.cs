using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public int initialLifeCount = 100;
    public int initialMoneyCount = 1000;

    private InputMode _inputMode;
    private int _currentPlacingTower;
    private Transform _currentlySelectedObject;

    [SerializeField] private int _moneyCount;
    [SerializeField] private int _lifeCount;

    private PlacementController _placementController;
    private WaveSpawner _waveSpawner;
    private UIController _uiController;
    private SelectionController _selectionController;

    public float timePlayed = 0f;

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
        _selectionController = GetComponent<SelectionController>();
        _waveSpawner = GetComponent<WaveSpawner>();
        _uiController = GetComponent<UIController>();

        // initial game state
        gameState = GameState.Running;
        _inputMode = InputMode.Play;
        _currentPlacingTower = -1;
        _currentlySelectedObject = null;

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
                _uiController.hideTowerPanel();
                _uiController.hideGameOverUI();
                _uiController.hideTowerPanel();
                updateUI();
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
                    
                }

                break;
            case GameState.GameOver:
                _uiController.showGameOverUI(_waveIndex);

                enabled = false;
                break;
            case GameState.Finished:
                _uiController.gameOverText.text = "Finished";
                _uiController.gameOverText.enabled = true;
                _uiController.lifeCountText.enabled = false;

                enabled = false;
                break;
        }
    }

    private void updateUI()
    {
        if (_currentlySelectedObject != null)
        {
            string name = _currentlySelectedObject.gameObject.name;
            _uiController.showTowerPanel(name, 4);
        }
        _uiController.updateUI(_countdownWave, _lifeCount, _moneyCount, timePlayed);
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        gameState = GameState.Running;
    }

    public void Menu()
    {
        Debug.Log("MENU");
    }

    public void ExitPlacementMode()
    {
        _inputMode = InputMode.Play;
        _currentPlacingTower = -1;
        _placementController.SetPlacementModeInactive();
        _selectionController.SetSelectionModeActive();
    }

    public void SetToPlacementMode(int towerNumber)
    {
        if (_inputMode == InputMode.Play)
        {
            _selectionController.SetSelectionModeInactive();
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
        ExitPlacementMode();
    }

    public void RegisterKill(int earning)
    {
        _moneyCount += earning;
    }

    public void SetSelectedObject(Transform o)
    {
        _currentlySelectedObject = o;
    }

    public void SellCurrentSelectedTower()
    {
        _moneyCount += 200;
        Destroy(_currentlySelectedObject.gameObject);
    }
    
}