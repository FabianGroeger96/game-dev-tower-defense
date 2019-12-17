﻿using System;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("GameOver")] public Text gameOverText;
    public Text roundText;
    public GameObject gameOverUI;

    [Header("Counters")] public GameObject counterPanel;
    public Text waveCountdownText;
    public Text waveCountText;

    [Header("Stats")] public Text lifeCountText;
    public Text killedCountText;
    public Text moneyCountText;
    public Text timeCountText;

    [Header("Info Panel")] public GameObject infoPanel;
    public GameObject actionTowerPanel;
    public GameObject changeTowerPanel;
    public Text towerNameText;
    public Text towerLevelText;
    public Image healthBar;

    public Button buttonUpgrade;
    public Button buttonSell;

    public Button buttonNearestEnemy;
    public Button buttonFirstEnemy;
    public Button buttonLowestEnemy;
    public Button buttonHighestEnemy;
    
    [Header("GameUtils")]
    public GameObject pausePanel;
    public GameObject healthBarToggler;
    public static bool showHealthBars = false;
    public Text gameSpeedText;

    private float minutes;
    private float seconds;
    
    private float timeScaleBefore = 0f;

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.P))
        {
            togglePauseMenu();
        }
    }

    public void togglePauseMenu()
    {
        pausePanel.SetActive(!pausePanel.activeSelf);
        if (pausePanel.activeSelf)
        {
            timeScaleBefore = Time.timeScale;
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = timeScaleBefore;
        }
    }

    public float updateUI(float countdownWave, int waveCount, int waveMaxCount, bool waveRunning, int lifeCount,
        int killedCount,
        int moneyCount, float timePlayed)
    {
        // update UI elements
        if (waveRunning)
        {
            waveCountdownText.text = "THEY'RE COMMING!";
            waveCountdownText.fontSize = 28;
            waveCountText.text = "wave " + waveCount + " of " + waveMaxCount;
        }
        else
        {
            waveCountdownText.text = Mathf.Round(countdownWave).ToString();
            waveCountdownText.fontSize = 40;
            waveCountText.text = "until wave " + (waveCount + 1) + " of " + waveMaxCount;
        }

        // update UI stats
        updateStats(lifeCount, killedCount, moneyCount);

        if (showHealthBars)
        {
            healthBarToggler.GetComponentInChildren<Text>().text = "Hide Health Bars";
        }
        else
        {
            healthBarToggler.GetComponentInChildren<Text>().text = "Show Health Bars";
        }

        // evaluate how much time is elapsed
        timePlayed += Time.deltaTime;
        minutes = Mathf.Floor(timePlayed / 60);
        seconds = timePlayed % 60;
        if (seconds > 59) seconds = 59;
        if (minutes < 0)
        {
            minutes = 0;
            seconds = 0;
        }

        // update time played
        timeCountText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        
        // update game speed
        gameSpeedText.text = Math.Round(Time.timeScale, 2) + "x";

        return timePlayed;
    }

    public void showGameOverUI(int rounds)
    {
        gameOverUI.SetActive(true);
        gameOverText.text = "GAME OVER";
        roundText.text = rounds.ToString();

        counterPanel.SetActive(false);
        lifeCountText.enabled = false;
    }

    public void hideGameOverUI()
    {
        gameOverUI.SetActive(false);
        counterPanel.SetActive(true);
        lifeCountText.enabled = true;
    }

    public void showTowerPanel(GameObject gameObject)
    {
        infoPanel.SetActive(true);
        if (gameObject.GetComponent<Tower>())
        {
            Tower tower = gameObject.GetComponent<Tower>();
            towerNameText.text = tower.name;
            towerLevelText.text = "lvl: " + tower.level;
            healthBar.fillAmount = tower.health;
            
            buttonUpgrade.gameObject.GetComponentInChildren<Text>().text = "Upgrade \n" + "($" + tower.upgradeCost + ")";
            buttonSell.gameObject.GetComponentInChildren<Text>().text = "Sell \n" + "($" + tower.sellValue + ")";
            
            showTowerActionPanel((int) tower.getTargetFinderMode());
        }
        else if (gameObject.GetComponent<Enemy>())
        {
            Enemy enemy = gameObject.GetComponent<Enemy>();
            towerNameText.text = enemy.name;
            towerLevelText.text = "lvl: " + enemy.level;
            healthBar.fillAmount = enemy.health;
        }
    }

    public void hideTowerPanel()
    {
        infoPanel.SetActive(false);
        actionTowerPanel.SetActive(false);
        changeTowerPanel.SetActive(false);

        buttonNearestEnemy.gameObject.SetActive(false);
        buttonFirstEnemy.gameObject.SetActive(false);
        buttonLowestEnemy.gameObject.SetActive(false);
        buttonHighestEnemy.gameObject.SetActive(false);
    }

    public void showTowerActionPanel(int towerAction)
    {
        actionTowerPanel.SetActive(true);
        changeTowerPanel.SetActive(true);

        buttonNearestEnemy.gameObject.SetActive(true);
        buttonFirstEnemy.gameObject.SetActive(true);
        buttonLowestEnemy.gameObject.SetActive(true);
        buttonHighestEnemy.gameObject.SetActive(true);

        buttonNearestEnemy.interactable = true;
        buttonFirstEnemy.interactable = true;
        buttonLowestEnemy.interactable = true;
        buttonHighestEnemy.interactable = true;

        switch (towerAction)
        {
            case 0:
                buttonNearestEnemy.GetComponent<Image>().color = Color.blue;
                buttonNearestEnemy.gameObject.GetComponentInChildren<Text>().color = Color.white;

                buttonFirstEnemy.GetComponent<Image>().color = Color.gray;
                buttonFirstEnemy.gameObject.GetComponentInChildren<Text>().color = Color.black;

                buttonLowestEnemy.GetComponent<Image>().color = Color.gray;
                buttonLowestEnemy.gameObject.GetComponentInChildren<Text>().color = Color.black;

                buttonHighestEnemy.GetComponent<Image>().color = Color.gray;
                buttonHighestEnemy.gameObject.GetComponentInChildren<Text>().color = Color.black;
                break;
            case 1:
                buttonNearestEnemy.GetComponent<Image>().color = Color.gray;
                buttonNearestEnemy.gameObject.GetComponentInChildren<Text>().color = Color.black;

                buttonFirstEnemy.GetComponent<Image>().color = Color.blue;
                buttonFirstEnemy.gameObject.GetComponentInChildren<Text>().color = Color.white;

                buttonLowestEnemy.GetComponent<Image>().color = Color.gray;
                buttonLowestEnemy.gameObject.GetComponentInChildren<Text>().color = Color.black;

                buttonHighestEnemy.GetComponent<Image>().color = Color.gray;
                buttonHighestEnemy.gameObject.GetComponentInChildren<Text>().color = Color.black;
                break;
            case 2:
                buttonNearestEnemy.GetComponent<Image>().color = Color.gray;
                buttonNearestEnemy.gameObject.GetComponentInChildren<Text>().color = Color.black;

                buttonFirstEnemy.GetComponent<Image>().color = Color.gray;
                buttonFirstEnemy.gameObject.GetComponentInChildren<Text>().color = Color.black;

                buttonLowestEnemy.GetComponent<Image>().color = Color.blue;
                buttonLowestEnemy.gameObject.GetComponentInChildren<Text>().color = Color.white;

                buttonHighestEnemy.GetComponent<Image>().color = Color.gray;
                buttonHighestEnemy.gameObject.GetComponentInChildren<Text>().color = Color.black;
                break;
            case 3:
                buttonNearestEnemy.GetComponent<Image>().color = Color.gray;
                buttonNearestEnemy.gameObject.GetComponentInChildren<Text>().color = Color.black;

                buttonFirstEnemy.GetComponent<Image>().color = Color.gray;
                buttonFirstEnemy.gameObject.GetComponentInChildren<Text>().color = Color.black;

                buttonLowestEnemy.GetComponent<Image>().color = Color.gray;
                buttonLowestEnemy.gameObject.GetComponentInChildren<Text>().color = Color.black;

                buttonHighestEnemy.GetComponent<Image>().color = Color.blue;
                buttonHighestEnemy.gameObject.GetComponentInChildren<Text>().color = Color.white;
                break;
        }
    }

    public void updateStats(int lifeCount, int killedCount, int moneyCount)
    {
        lifeCountText.text = "Life: " + lifeCount;
        killedCountText.text = "Killed: " + killedCount;
        moneyCountText.text = "$" + moneyCount;
    }

    public void setWaveCountText(string text)
    {
        waveCountdownText.text = text;
    }

    public void setLifeCountText(string text)
    {
        lifeCountText.text = "Life: " + text;
    }

    public void setMoneyCountText(string text)
    {
        moneyCountText.text = "$" + text;
    }

    public void changeHealthBarOption()
    {
        showHealthBars = !showHealthBars;
    }
}