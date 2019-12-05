using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Text waveCountdownText;
    public Text lifeCountText;

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

    public void setLifeCountText(string text)
    {
        _lifeCount = text;
    }

    public void setWaveCountText(string text)
    {
        _waveCount = text;
    }
}