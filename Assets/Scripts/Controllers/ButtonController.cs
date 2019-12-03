using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonController : BaseController
{
    
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void PlaceTower(int towerId)
    {
        Debug.Log("Place tower: " + towerId);
        _gc.SetToPlacementMode(towerId);
    }
}