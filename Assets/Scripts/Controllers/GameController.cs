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

    public GameState gameState = GameState.Init;
    public Text lifeCountText;
    public Text gameOverText;
    public int initialLifeCount = 100;

    private int _lifeCount;

    // Start is called before the first frame update
    void Start()
    {
        // initial game state
        gameState = GameState.Running;

        // initial life count
        _lifeCount = initialLifeCount;
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

    public void RemoveLifeCount(int lives)
    {
        _lifeCount -= lives;
        if (_lifeCount <= 0)
        {
            Debug.Log("Game Over");
            gameState = GameState.GameOver;
        }
    }
}