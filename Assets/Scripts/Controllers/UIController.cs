using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("GameOver")]
    public Text gameOverText;
    public Text roundText;
    public GameObject gameOverUI;

    [Header("Counters")] public GameObject counterPanel;
    public Text waveCountdownText;
    public Text timeCountText;

    [Header("Stats")]
    public Text lifeCountText;
    public Text killedCountText;
    public Text moneyCountText;

    [Header("Info Panel")] 
    public GameObject infoPanel;
    public GameObject actionTowerPanel;
    public GameObject changeTowerPanel;
    public Text towerNameText;
    public Text towerLevelText;
    public Image healthBar;
    
    private float minutes;
    private float seconds;

    public GameObject healthBarToggler;
    public static bool showHealthBars = false;
    
    public float updateUI(float countdownWave, bool waveRunning, int lifeCount, int killedCount, int moneyCount, float timePlayed)
    {
        // update UI elements
        if (waveRunning)
        {
            waveCountdownText.text = "THEY'RE COMMING!";
            waveCountdownText.fontSize = 28;
        }
        else
        {
            waveCountdownText.text = Mathf.Round(countdownWave).ToString();
            waveCountdownText.fontSize = 40;
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

    public void showTowerPanel(string towerName, int towerLevel, float health)
    {
        infoPanel.SetActive(true);
        towerNameText.text = towerName;
        towerLevelText.text = "lvl: " + towerLevel;
        healthBar.fillAmount = health;
    }
    
    public void hideTowerPanel()
    {
        infoPanel.SetActive(false);
        actionTowerPanel.SetActive(false);
        changeTowerPanel.SetActive(false);
    }

    public void showTowerActionPanel()
    {
        actionTowerPanel.SetActive(true);
        changeTowerPanel.SetActive(true);
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