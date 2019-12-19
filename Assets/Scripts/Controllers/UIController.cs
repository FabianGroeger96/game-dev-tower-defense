using System;
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
    private Text _buttonNearestEnemyText;
    private Image _buttonNearestEnemyImage;
    
    public Button buttonFirstEnemy;
    private Image _buttonFirstEnemyImage;
    private Text _buttonFirstEnemyText;
    
    public Button buttonLowestEnemy;
    private Image _buttonLowestEnemyImage;
    private Text _buttonLowestEnemyText;
    
    public Button buttonHighestEnemy;
    private Image _buttonHighestEnemyImage;
    private Text _buttonHighestEnemyText;
    
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
        _buttonHighestEnemyText = buttonHighestEnemy.gameObject.GetComponentInChildren<Text>();
        _buttonHighestEnemyImage = buttonHighestEnemy.GetComponent<Image>();
        _buttonLowestEnemyText = buttonLowestEnemy.gameObject.GetComponentInChildren<Text>();
        _buttonLowestEnemyImage = buttonLowestEnemy.GetComponent<Image>();
        _buttonFirstEnemyText = buttonFirstEnemy.gameObject.GetComponentInChildren<Text>();
        _buttonFirstEnemyImage = buttonFirstEnemy.GetComponent<Image>();
        _buttonNearestEnemyImage = buttonNearestEnemy.GetComponent<Image>();
        _buttonNearestEnemyText = buttonNearestEnemy.gameObject.GetComponentInChildren<Text>();
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

    public float UpdateUi(float countdownWave, int waveCount, int waveMaxCount, bool waveRunning, int lifeCount,
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

    public void ShowTowerPanel(int money, GameObject gameObject)
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

            buttonUpgrade.interactable = !(money <= tower.upgradeCost);
            
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
                _buttonNearestEnemyImage.color = Color.blue;
                _buttonNearestEnemyText.color = Color.white;

                _buttonFirstEnemyImage.color = Color.gray;
                _buttonFirstEnemyText.color = Color.black;

                _buttonLowestEnemyImage.color = Color.gray;
                _buttonLowestEnemyText.color = Color.black;

                _buttonHighestEnemyImage.color = Color.gray;
                _buttonHighestEnemyText.color = Color.black;
                break;
            case 1:
                _buttonNearestEnemyImage.color = Color.gray;
                _buttonNearestEnemyText.color = Color.black;

                _buttonFirstEnemyImage.color = Color.blue;
                _buttonFirstEnemyText.color = Color.white;

                _buttonLowestEnemyImage.color = Color.gray;
                _buttonLowestEnemyText.color = Color.black;

                _buttonHighestEnemyImage.color = Color.gray;
                _buttonHighestEnemyText.color = Color.black;
                break;
            case 2:
                _buttonNearestEnemyImage.color = Color.gray;
                _buttonNearestEnemyText.color = Color.black;

                _buttonFirstEnemyImage.color = Color.gray;
                _buttonFirstEnemyText.color = Color.black;

                _buttonLowestEnemyImage.color = Color.blue;
                _buttonLowestEnemyText.color = Color.white;

                _buttonHighestEnemyImage.color = Color.gray;
                _buttonHighestEnemyText.color = Color.black;
                break;
            case 3:
                _buttonNearestEnemyImage.color = Color.gray;
                _buttonNearestEnemyText.color = Color.black;

                _buttonFirstEnemyImage.color = Color.gray;
                _buttonFirstEnemyText.color = Color.black;

                _buttonLowestEnemyImage.color = Color.blue;
                _buttonLowestEnemyText.color = Color.white;

                _buttonHighestEnemyImage.color = Color.gray;
                _buttonHighestEnemyText.color = Color.black;
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

    public void ToogleHealthBarOption()
    {
        showHealthBars = !showHealthBars;
    }

    public void ToogleSoundOption()
    {
        SoundController.ToogleMute();
    }
    
}