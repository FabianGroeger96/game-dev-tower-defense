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
    public Text lifeCountText;
    public Text gameOverText;
    public int initialLifeCount = 100;
    public int initialMoneyCount = 1000;

    private InputMode _inputMode;
    private int _currentPlacingTower;
    
    private int _moneyCount;
    private int _lifeCount;

    private PlacementController _placementController;

    // Start is called before the first frame update
    void Start()
    {
        //Get controllers
        _placementController = GetComponent<PlacementController>();
        
        // initial game state
        gameState = GameState.Running;
        _inputMode = InputMode.Play;
        _currentPlacingTower = -1;

        // initial life count
        _lifeCount = initialLifeCount;
        _moneyCount = initialMoneyCount;
        lifeCountText.text = _lifeCount.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState == GameState.GameOver)
        {
            gameOverText.enabled = true;
            lifeCountText.enabled = false;
        }
        else
        {
            gameOverText.enabled = false;
            lifeCountText.enabled = true;

            // update life count
            lifeCountText.text = _lifeCount.ToString();
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
        Debug.Log(_moneyCount);
    }
}