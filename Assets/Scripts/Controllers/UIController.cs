using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Text waveCountdownText;
    public Text lifeCountText;
    public Text moneyCountText;
    public Text timeCountText;

    private string _waveCount;
    private string _lifeCount;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
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