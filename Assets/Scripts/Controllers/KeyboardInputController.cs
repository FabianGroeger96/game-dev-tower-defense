using UnityEngine;

/// <summary>
/// Class used to get the inputs of the keyboard and delegate to the GameManager.
/// </summary>
public class KeyboardInputController : MonoBehaviour
{
    // current mode of the game
    private int _currentMode = 0; // 0: Non-placement, 1: Placement

    // reference to the GameManager to delegate
    private GameManager _gc;

    /// <summary>
    /// Awake is being used to initialize all the reference the class needs,
    /// and to bring it to an initial state.
    /// </summary>
    private void Awake()
    {
        _gc = GetComponent<GameManager>();
    }

    /// <summary>
    /// Within every frame it checks if there is an input from the keyboard.
    /// </summary>
    private void Update()
    {
        CheckKeyboardInput();
    }

    /// <summary>
    /// Checks if there is an input from the keyboard.
    /// 1 - place tower "roller"
    /// 2 - place tower "parabolic"
    /// 3 - place tower "laser"
    /// ESC or P - toggles pause menu
    /// </summary>
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
        
        if (Input.GetKeyUp(KeyCode.E))
        {
            _gc.SetSelectedObject(null);
        }
        
        if (Input.GetKeyUp(KeyCode.R))
        {
            _gc.ResetCameraPosition();
        }
        
        
    }
}