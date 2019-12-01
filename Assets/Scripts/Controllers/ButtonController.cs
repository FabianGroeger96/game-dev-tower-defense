using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    private GameController _gameController;
    private PlacementController _placementController;

    // Start is called before the first frame update
    void Start()
    {
        _gameController = GameObject.Find("GameController").GetComponent<GameController>();
        _placementController = _gameController.GetComponent<PlacementController>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void PlaceTower(int towerId)
    {
        Debug.Log("Place tower: " + towerId);
        _placementController.placeMode(towerId);
    }
}