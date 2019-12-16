using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerPlacementButtonController : BaseController
{
    public Button placementButton;
    public GameObject tower;

    // Start is called before the first frame update
    void Start()
    {
        Init();
        Tower tower = this.tower.GetComponent<Tower>();
        placementButton.gameObject.GetComponentInChildren<Text>().text = tower.name + "\n" + "($" + tower.costs + ")";
    }

    // Update is called once per frame
    void Update()
    {
        if (_gc.moneyCount >= tower.GetComponent<Tower>().costs)
        {
            placementButton.interactable = true;
        }
        else
        {
            placementButton.interactable = false;
            placementButton.GetComponent<Image>().color = Color.gray;
        }
    }

    public void PlaceTower(int towerId)
    {
        _gc.SetToPlacementMode(towerId);
    }
}