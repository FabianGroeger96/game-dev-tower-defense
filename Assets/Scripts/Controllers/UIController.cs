using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Text gameOverText;
    
    [Header("Counters")]
    public Text waveCountdownText;
    public Text timeCountText;

    [Header("Stats")]
    public Text lifeCountText;
    public Text moneyCountText;

    [Header("Tower Panel")] 
    public GameObject towerPanel;
    public Text towerNameText;
    public Text towerLevelText;
    
    private float minutes;
    private float seconds;
    
    public void updateUI(float countdownWave, int lifeCount, int moneyCount, float timePlayed)
    {
        // update UI elements
        setWaveCountText(Mathf.Round(countdownWave).ToString());
        setLifeCountText(lifeCount.ToString());
        setMoneyCountText(moneyCount.ToString());

        timePlayed += Time.deltaTime;
        minutes = Mathf.Floor(timePlayed / 60);
        seconds = timePlayed % 60;
        if (seconds > 59) seconds = 59;
        if (minutes < 0)
        {
            minutes = 0;
            seconds = 0;
        }

        setTimeCountText(string.Format("{0:0}:{1:00}", minutes, seconds));
    }

    public void showTowerPanel(string towerName, int towerLevel)
    {
        towerPanel.SetActive(true);
        towerNameText.text = towerName;
        towerLevelText.text = "lvl: " + towerLevel;
    }

    public void hideTowerPanel()
    {
        towerPanel.SetActive(false);
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

    public void setTimeCountText(string text)
    {
        timeCountText.text = text;
    }
}