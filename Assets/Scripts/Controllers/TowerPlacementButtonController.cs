using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerPlacementButtonController : BaseController
{
    public Button placementButton;
    private Image _placementButtonImage;
    private Text _placementButtonText;

    public GameObject tower;
    private Tower _tower;

    // Start is called before the first frame update
    private void Awake()
    {
        _placementButtonImage = placementButton.GetComponent<Image>();
        _placementButtonText = placementButton.gameObject.GetComponentInChildren<Text>();
        _tower = tower.GetComponent<Tower>();
    }

    private void Start()
    {
        Init();
        _placementButtonText.text = _tower.name + "\n" + "($" + _tower.costs + ")";
    }

    // Update is called once per frame
    private void Update()
    {
        if (_gc.moneyCount >= _tower.costs)
        {
            placementButton.interactable = true;
        }
        else
        {
            placementButton.interactable = false;
            _placementButtonImage.color = Color.gray;
        }
    }

    public void PlaceTower(int towerId)
    {
        _gc.SetToPlacementMode(towerId);
    }
}