using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{

    private int _currentMode = 0; // 0: Non-placement, 1: Placement
    private PlacementController _placementController;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _placementController = GetComponent<PlacementController>();
    }

    // Update is called once per frame
    void Update()
    {
        checkKeyboardInput();
    }

    private void checkKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _placementController.placeMode(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _placementController.placeMode(2);
        }
    }

    
    
    
}
