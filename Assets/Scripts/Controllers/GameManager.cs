using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// The GameManager of the game, which controls the whole game and holds the most
/// relevant informations about the state of the game.
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Represents the state of the game.
    /// </summary>
    public enum GameState
    {
        Init,
        Running,
        GameOver,
        Finished
    };

    /// <summary>
    /// Represents the Input mode of the game.
    /// </summary>
    private enum InputMode
    {
        Play,
        Place
    }
    
    // contains all the avaible towers in the game
    [SerializeField] private Tower[] towers;
    
    // controls the game state
    public GameState gameState = GameState.Init;
    
    // initial money count of the game
    public int initialMoneyCount = 1000;
    // actual money count of the game
    [SerializeField] public int moneyCount;
    
    // reference to the base
    public Base ownBase;
    
    // state of the input mode
    private InputMode _inputMode;
    // id of the current tower to place
    private int _currentPlacingTower;
    // reference to the currently selected object
    private Transform _currentlySelectedObject;
    
    // reference to the placement controller
    private PlacementController _placementController;
    // reference to the wave spawner
    private WaveSpawner _waveSpawner;
    // reference to the ui controller
    private UIController _uiController;
    // reference to the selection controller
    private SelectionController _selectionController;
    
    // keeps track of how long the game is being playd
    public float timePlayed = 0f;

    // variables for spawning enemies
    private float _countdownWave = 2;
    private int _waveIndex = 0;
    private bool _waveRunning = false;
    
    // keeps track of the time scale before pause / menu
    private float _timeScaleBefore = 0f;
    
    // contain all the waves of the game
    public Wave[] waves;
    
    // how long to wait until the next wave is being spawned
    public float timeBetweenWaves = 5;
    // how many enemies are alive
    public static int enemiesAlive = 0;
    // how many enemies are killed
    public static int enemiesKilled = 0;
    
    /// <summary>
    /// Awake is being used to initialize all the reference the class needs,
    /// and to bring it to an initial state.
    /// </summary>
    private void Awake()
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
    
    /// <summary>
    /// Within every frame the game manager checks the state of the game
    /// and delegates the actions to the other controllers.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when the gamestate is not a value from the enum</exception>
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
                UpdateUI();

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
                    else if (_waveIndex >= waves.Length)
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
                Time.timeScale = 0.25f;
                break;
            case GameState.Finished:
                _uiController.ShowGameOverUi(_waveIndex);

                _uiController.gameOverText.text = "Finished";
                _uiController.gameOverText.enabled = true;
                _uiController.lifeCountText.enabled = false;

                enabled = false;
                Time.timeScale = 0.25f;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    /// <summary>
    /// Updates the UI according to the state of the game.
    /// </summary>
    private void UpdateUI()
    {
        if (_currentlySelectedObject != null)
        {
            _uiController.ShowTowerPanel(moneyCount, _currentlySelectedObject.gameObject);
        }

        var lifeCount = (int) ownBase.health;
        timePlayed = _uiController.UpdateUI(_countdownWave, _waveIndex, waves.Length, _waveRunning, lifeCount,
            enemiesKilled, moneyCount,
            timePlayed);
    }
    
    /// <summary>
    /// Restarts the game and resets all variables.
    /// </summary>
    public void Retry()
    {
        _uiController.TogglePauseMenu();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
        _waveIndex = 0;
        _waveRunning = false;
        enemiesAlive = 0;
        enemiesKilled = 0;
        gameState = GameState.Running;
    }
    
    /// <summary>
    /// Loads the "Main Menu"
    /// </summary>
    public void Menu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }
    
    /// <summary>
    /// Loads the "Main Level" and starts the game.
    /// </summary>
    public void Play()
    {
        SceneManager.LoadScene("MainLevel");
        Time.timeScale = 1;
        _waveIndex = 0;
        _waveRunning = false;
        enemiesAlive = 0;
        enemiesKilled = 0;
        gameState = GameState.Running;
    }
    
    /// <summary>
    /// Quits the Application.
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }
    
    /// <summary>
    /// Toggles the pause of the game.
    /// </summary>
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
    
    /// <summary>
    /// Speeds up the game.
    /// </summary>
    public void MakeGameFaster()
    {
        if (Time.timeScale < 5)
        {
            Time.timeScale += 0.25f;
        }
    }
    
    /// <summary>
    /// Slows game down.
    /// </summary>
    public void MakeGameSlower()
    {
        if (Time.timeScale > 0.25)
        {
            Time.timeScale -= 0.25f;
        }
    }
    
    /// <summary>
    /// Toggles pause menu or exits place mode, when in place mode.
    /// </summary>
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
    
    /// <summary>
    /// Exits placement mode.
    /// </summary>
    private void ExitPlacementMode()
    {
        _inputMode = InputMode.Play;
        _currentPlacingTower = -1;
        _placementController.SetPlacementModeInactive();
        _selectionController.SetSelectionModeActive();
    }

    /// <summary>
    /// Exists the selection mode, to hide the info panel.
    /// </summary>
    public void ExitSelectionMode()
    {
        SetSelectedObject(null);
    }
    
    /// <summary>
    /// Changes to placement mode to place the given tower on the map.
    /// </summary>
    /// <param name="towerNumber">tower to place</param>
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
    
    /// <summary>
    /// When the tower was placed, the money count has to be updated.
    /// </summary>
    /// <param name="costs">cost of the placed tower</param>
    public void TowerPlaced(int costs)
    {
        moneyCount -= towers[_currentPlacingTower - 1].costs;
        ExitPlacementMode();
    }
    
    /// <summary>
    /// Registers a kill of an enemy.
    /// </summary>
    /// <param name="earning">earning of the kill</param>
    public void RegisterKill(int earning)
    {
        moneyCount += earning;
    }
    
    /// <summary>
    /// Sets the selected object to the given one.
    /// </summary>
    /// <param name="o">selected object.</param>
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
    
    /// <summary>
    /// Sells the currently selected tower and updates the money count.
    /// </summary>
    public void SellCurrentSelectedTower()
    {
        var selectedObject = _currentlySelectedObject.gameObject;
        var tower = selectedObject.GetComponentInChildren<Tower>();
        moneyCount += (int) tower.sellValue;
        Destroy(_currentlySelectedObject.gameObject);
    }
    
    /// <summary>
    /// Updates the currently selected tower and updates the money count.
    /// </summary>
    public void UpgradeCurrentSelectedTower()
    {
        var selectedObject = _currentlySelectedObject.gameObject;
        var tower = selectedObject.GetComponent<Tower>();
        if (0 <= moneyCount - tower.upgradeCost)
        {
            moneyCount -= (int) tower.upgradeCost;
            tower.UpgradeTower();
            // play upgrade effect
            var effect = Instantiate(tower.upgradeEffect, tower.transform.position,
                Quaternion.Euler(270f, 0f, 0f));
            Destroy(effect, 1f);
        }
    }
    
    /// <summary>
    /// Changes the target finder action of the selected tower to the given one.
    /// </summary>
    /// <param name="action">target finder action</param>
    public void ChangeTowerAction(int action)
    {
        var selectedObject = _currentlySelectedObject.gameObject;
        var tower = selectedObject.GetComponent<Tower>();

        var targetFinderMode = (TargetFinder.TargetFinderMode) action;
        tower.ChangeTargetFinderMode(targetFinderMode);
    }
}