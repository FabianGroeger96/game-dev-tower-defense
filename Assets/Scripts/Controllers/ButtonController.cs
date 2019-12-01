using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : MonoBehaviour
{
    private PlacementController _placementController;
    public GameObject gameController;

    // Start is called before the first frame update
    void Start()
    {
        _placementController = gameController.GetComponent<PlacementController>();
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