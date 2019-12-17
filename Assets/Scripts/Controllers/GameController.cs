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

    [SerializeField] public int moneyCount;
    [SerializeField] private int _lifeCount;

    private PlacementController _placementController;
    private WaveSpawner _waveSpawner;
    private UIController _uiController;
    private SelectionController _selectionController;

    public float timePlayed = 0f;

    // variables for spawning enemies
    private float _countdownWave = 2;
    private int _waveIndex = 0;
    private bool _waveRunning = false;

    private float timeScaleBefore = 0f;

    public Wave[] waves;

    public float timeBetweenWaves = 5;
    public static int enemiesAlive = 0;
    public static int enemiesKilled = 0;

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
        moneyCount = initialMoneyCount;

        // initial UI Elements
        _uiController.setLifeCountText(_lifeCount.ToString());
        _uiController.setWaveCountText(_countdownWave.ToString());
        _uiController.setMoneyCountText(moneyCount.ToString());
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

                if (enemiesAlive <= 0)
                {
                    if (_countdownWave <= 0f)
                    {
                        // Call subprocess
                        Wave wave = waves[_waveIndex];
                        StartCoroutine(_waveSpawner.SpawnWave(wave));

                        _waveIndex++;
                        _countdownWave = timeBetweenWaves;
                    }
                    else if (_waveIndex > waves.Length)
                    {
                        gameState = GameState.Finished;
                        break;
                    }

                    _waveRunning = false;
                    _countdownWave -= Time.deltaTime;
                }
                else
                {
                    _waveRunning = true;
                }

                break;
            case GameState.GameOver:
                _uiController.showGameOverUI(_waveIndex);

                enabled = false;
                break;
            case GameState.Finished:
                _uiController.showGameOverUI(_waveIndex);

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
            _uiController.showTowerPanel(_currentlySelectedObject.gameObject);
        }

        timePlayed = _uiController.updateUI(_countdownWave, _waveIndex, waves.Length, _waveRunning, _lifeCount,
            enemiesKilled, moneyCount,
            timePlayed);
    }

    public void Retry()
    {
        _uiController.togglePauseMenu();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        _waveIndex = 0;
        _waveRunning = false;
        enemiesAlive = 0;
        enemiesKilled = 0;
        gameState = GameState.Running;
    }

    public void Menu()
    {
        Time.timeScale = 1f;
        Debug.Log("MENU");
    }

    public void Play()
    {
        SceneManager.LoadScene("MainLevel");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void MakeGamePause()
    {
        if (Time.timeScale == 0)
        {
            Time.timeScale = timeScaleBefore;
        }
        else
        {
            timeScaleBefore = Time.timeScale;
            Time.timeScale = 0;
        }
    }

    public void MakeGameFaster()
    {
        if (Time.timeScale < 5)
        {
            Time.timeScale += 0.5f;
        }
    }

    public void MakeGameSlower()
    {
        if (Time.timeScale > 0.25)
        {
            Time.timeScale -= 0.25f;
        }
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
            if (_tower.costs <= moneyCount)
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
            gameState = GameState.GameOver;
        }
    }

    public void TowerPlaced(int costs)
    {
        moneyCount -= _towers[_currentPlacingTower - 1].costs;
        ExitPlacementMode();
    }

    public void RegisterKill(int earning)
    {
        moneyCount += earning;
    }

    public void SetSelectedObject(Transform o)
    {
        if (_currentlySelectedObject != null)
        {
            SelectableObject soc = _currentlySelectedObject.GetComponent<SelectableObject>();
            if(soc)
            {
                soc.IsUnselected();
            }
        }
        _currentlySelectedObject = o.root;
        SelectableObject soc_new = _currentlySelectedObject.GetComponent<SelectableObject>();
        if(soc_new)
        {
            soc_new.IsSelected();
        }
    }

    public void SellCurrentSelectedTower()
    {
        GameObject selectedObject = _currentlySelectedObject.gameObject;
        Tower tower = selectedObject.GetComponentInChildren<Tower>();
        moneyCount += (int) tower.sellValue;
        Destroy(_currentlySelectedObject.gameObject);
    }

    public void UpgradeCurrentSelectedTower()
    {
        GameObject selectedObject = _currentlySelectedObject.gameObject;
        Tower tower = selectedObject.GetComponent<Tower>();
        if (0 < moneyCount - tower.upgradeCost)
        {
            moneyCount -= (int) tower.upgradeCost;
            tower.UpgradeTower();
            // play upgrade effect
            GameObject effect = Instantiate(tower.upgradeEffect, tower.transform.position,
                Quaternion.Euler(270f, 0f, 0f));
            Destroy(effect, 1f);
        }
    }

    public void ChangeTowerAction(int action)
    {
        GameObject selectedObject = _currentlySelectedObject.gameObject;
        Tower tower = selectedObject.GetComponent<Tower>();

        TargetFinder.TargetFinderMode targetFinderMode = (TargetFinder.TargetFinderMode) action;
        tower.ChangeTargetFinderMode(targetFinderMode);
    }
}