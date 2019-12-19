using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardInputController : MonoBehaviour
{

    private int _currentMode = 0; // 0: Non-placement, 1: Placement
    private GameManager _gc;
    
    
    // Start is called before the first frame update
    private void Awake()
    {
        _gc = GetComponent<GameManager>();
    }

    // Update is called once per frame
    private void Update()
    {
        CheckKeyboardInput();
    }

    private void CheckKeyboardInput()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _gc.SetToPlacementMode(1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _gc.SetToPlacementMode(2);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _gc.SetToPlacementMode(3);
        }
        
        if (Input.GetKeyUp(KeyCode.Escape) || Input.GetKeyUp(KeyCode.P))
        {
            _gc.TogglePauseMenu();
        }
    }
}
