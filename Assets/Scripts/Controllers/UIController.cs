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
    
    private Text _healthBarTogglerText;
    private Text _buttonUpgradeText;
    private Text _buttonSellText;

    private float _minutes;
    private float _seconds;
    
    private float _timeScaleBefore = 0f;

    public void Awake()
    {
        _healthBarTogglerText = healthBarToggler.GetComponentInChildren<Text>();
        _buttonUpgradeText = buttonUpgrade.GetComponentInChildren<Text>();
        _buttonSellText = buttonSell.GetComponentInChildren<Text>();
    }

    public void TogglePauseMenu()
    {
        pausePanel.SetActive(!pausePanel.activeSelf);
        if (pausePanel.activeSelf)
        {
            _timeScaleBefore = Time.timeScale;
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = _timeScaleBefore;
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
        UpdateStats(lifeCount, killedCount, moneyCount);
        
        // update health bar toggler text
        _healthBarTogglerText.text = showHealthBars ? "Hide Health Bars" : "Show Health Bars";

        // evaluate how much time is elapsed
        timePlayed += Time.deltaTime;
        _minutes = Mathf.Floor(timePlayed / 60);
        _seconds = timePlayed % 60;
        if (_seconds > 59) _seconds = 59;
        if (_minutes < 0)
        {
            _minutes = 0;
            _seconds = 0;
        }

        // update time played
        timeCountText.text = $"{_minutes:00}:{_seconds:00}";
        
        // update game speed
        gameSpeedText.text = Math.Round(Time.timeScale, 2) + "x";

        return timePlayed;
    }

    public void ShowGameOverUi(int rounds)
    {
        gameOverUI.SetActive(true);
        gameOverText.text = "GAME OVER";
        roundText.text = rounds.ToString();

        counterPanel.SetActive(false);
        lifeCountText.enabled = false;
    }

    public void HideGameOverUi()
    {
        gameOverUI.SetActive(false);
        counterPanel.SetActive(true);
        lifeCountText.enabled = true;
    }

    public void ShowTowerPanel(GameObject gameObject)
    {
        infoPanel.SetActive(true);
        if (gameObject.GetComponent<Tower>())
        {
            Tower tower = gameObject.GetComponent<Tower>();
            towerNameText.text = tower.name;
            towerLevelText.text = "lvl: " + tower.level;
            healthBar.fillAmount = tower.health;
            
            _buttonUpgradeText.text = "Upgrade \n" + "($" + tower.upgradeCost + ")";
            _buttonSellText.text = "Sell \n" + "($" + tower.sellValue + ")";
            
            ShowTowerActionPanel((int) tower.getTargetFinderMode());
        }
        else if (gameObject.GetComponent<Enemy>())
        {
            Enemy enemy = gameObject.GetComponent<Enemy>();
            towerNameText.text = enemy.name;
            towerLevelText.text = "lvl: " + enemy.level;
            healthBar.fillAmount = enemy.health;
        }
    }

    public void HideTowerPanel()
    {
        infoPanel.SetActive(false);
        actionTowerPanel.SetActive(false);
        changeTowerPanel.SetActive(false);

        buttonNearestEnemy.gameObject.SetActive(false);
        buttonFirstEnemy.gameObject.SetActive(false);
        buttonLowestEnemy.gameObject.SetActive(false);
        buttonHighestEnemy.gameObject.SetActive(false);
    }

    private void ShowTowerActionPanel(int towerAction)
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

    private void UpdateStats(int lifeCount, int killedCount, int moneyCount)
    {
        lifeCountText.text = "Life: " + lifeCount;
        killedCountText.text = "Killed: " + killedCount;
        moneyCountText.text = "$" + moneyCount;
    }

    public void SetWaveCountText(string text)
    {
        waveCountdownText.text = text;
    }

    public void SetLifeCountText(string text)
    {
        lifeCountText.text = "Life: " + text;
    }

    public void SetMoneyCountText(string text)
    {
        moneyCountText.text = "$" + text;
    }

    public void ChangeHealthBarOption()
    {
        showHealthBars = !showHealthBars;
    }
}