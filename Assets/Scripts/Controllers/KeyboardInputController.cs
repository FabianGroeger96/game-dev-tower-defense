﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInputController : MonoBehaviour
{

    private int _currentMode = 0; // 0: Non-placement, 1: Placement
    private GameController _gc;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _gc = GetComponent<GameController>();
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
            _gc.SetToPlacementMode(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _gc.SetToPlacementMode(2);
        }
    }

    
    
    
}
