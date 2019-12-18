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

    private enum InputMode
    {
        Play,
        Place
    }

    [SerializeField] private Tower[] towers;

    public GameState gameState = GameState.Init;

    public int initialMoneyCount = 1000;
    [SerializeField] public int moneyCount;

    public Base ownBase;

    private InputMode _inputMode;
    private int _currentPlacingTower;
    private Transform _currentlySelectedObject;

    private PlacementController _placementController;
    private WaveSpawner _waveSpawner;
    private UIController _uiController;
    private SelectionController _selectionController;

    public float timePlayed = 0f;

    // variables for spawning enemies
    private float _countdownWave = 2;
    private int _waveIndex = 0;
    private bool _waveRunning = false;

    private float _timeScaleBefore = 0f;

    public Wave[] waves;

    public float timeBetweenWaves = 5;
    public static int enemiesAlive = 0;
    public static int enemiesKilled = 0;

    private void Start()
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

        moneyCount = initialMoneyCount;

        // initial UI Elements
        _uiController.SetLifeCountText(ownBase.health.ToString());
        _uiController.SetWaveCountText(_countdownWave.ToString());
        _uiController.SetMoneyCountText(moneyCount.ToString());
    }

    private void Update()
    {
        switch (gameState)
        {
            case GameState.Init:
                break;

            case GameState.Running:
                _uiController.HideTowerPanel();
                _uiController.HideGameOverUi();
                _uiController.HideTowerPanel();
                UpdateUi();

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
                _uiController.ShowGameOverUi(_waveIndex);

                enabled = false;
                Time.timeScale = 0;
                break;
            case GameState.Finished:
                _uiController.ShowGameOverUi(_waveIndex);

                _uiController.gameOverText.text = "Finished";
                _uiController.gameOverText.enabled = true;
                _uiController.lifeCountText.enabled = false;

                enabled = false;
                Time.timeScale = 0;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void UpdateUi()
    {
        if (_currentlySelectedObject != null)
        {
            _uiController.ShowTowerPanel(moneyCount, _currentlySelectedObject.gameObject);
        }

        var lifeCount = (int) ownBase.health;
        timePlayed = _uiController.UpdateUi(_countdownWave, _waveIndex, waves.Length, _waveRunning, lifeCount,
            enemiesKilled, moneyCount,
            timePlayed);
    }

    public void Retry()
    {
        _uiController.TogglePauseMenu();
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
        SceneManager.LoadScene("MainMenu");
    }

    public void Play()
    {
        Time.timeScale = 1f;
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
            Time.timeScale = _timeScaleBefore;
        }
        else
        {
            _timeScaleBefore = Time.timeScale;
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

    public void TogglePauseMenu()
    {
        if (_inputMode == InputMode.Place)
        {
            ExitPlacementMode();
        }
        else
        {
            _uiController.TogglePauseMenu();
        }
    }

    private void ExitPlacementMode()
    {
        _inputMode = InputMode.Play;
        _currentPlacingTower = -1;
        _placementController.SetPlacementModeInactive();
        _selectionController.SetSelectionModeActive();
    }

    public void SetToPlacementMode(int towerNumber)
    {
        while (true)
        {
            if (_inputMode == InputMode.Play)
            {
                _selectionController.SetSelectionModeInactive();
                _inputMode = InputMode.Place;
                _currentPlacingTower = towerNumber;
                Tower _tower = towers[towerNumber - 1];
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
                    continue;
                }
                else
                {
                    ExitPlacementMode();
                }
            }

            break;
        }
    }

    public void TowerPlaced(int costs)
    {
        moneyCount -= towers[_currentPlacingTower - 1].costs;
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
            var soc = _currentlySelectedObject.GetComponent<SelectableObject>();
            if (soc)
            {
                soc.IsUnselected();
            }
        }

        if (o != null)
        {
            _currentlySelectedObject = o.root;
            var soc_new = _currentlySelectedObject.GetComponent<SelectableObject>();
            if (soc_new)
            {
                soc_new.IsSelected();
            }
        }
        else
        {
            _currentlySelectedObject = null;
        }
    }

    public void SellCurrentSelectedTower()
    {
        var selectedObject = _currentlySelectedObject.gameObject;
        var tower = selectedObject.GetComponentInChildren<Tower>();
        moneyCount += (int) tower.sellValue;
        Destroy(_currentlySelectedObject.gameObject);
    }

    public void UpgradeCurrentSelectedTower()
    {
        var selectedObject = _currentlySelectedObject.gameObject;
        var tower = selectedObject.GetComponent<Tower>();
        if (0 < moneyCount - tower.upgradeCost)
        {
            moneyCount -= (int) tower.upgradeCost;
            tower.UpgradeTower();
            // play upgrade effect
            var effect = Instantiate(tower.upgradeEffect, tower.transform.position,
                Quaternion.Euler(270f, 0f, 0f));
            Destroy(effect, 1f);
        }
    }

    public void ChangeTowerAction(int action)
    {
        var selectedObject = _currentlySelectedObject.gameObject;
        var tower = selectedObject.GetComponent<Tower>();

        var targetFinderMode = (TargetFinder.TargetFinderMode) action;
        tower.ChangeTargetFinderMode(targetFinderMode);
    }
}