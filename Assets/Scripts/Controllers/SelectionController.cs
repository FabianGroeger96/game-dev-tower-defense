using UnityEngine;

/// <summary>
/// Controller that keeps track of the currently selected object.
/// </summary>
public class SelectionController : MonoBehaviour
{
    // reference to the GameManager
    private GameManager _gc;

    // if a selection is currently taking place
    private bool _isActive;

    // reference to the currently selected object
    private Transform _currentObjectCandiate;

    /// <summary>
    /// Awake is being used to initialize all the reference the class needs,
    /// and to bring it to an initial state.
    /// </summary>
    void Awake()
    {
        _gc = GetComponent<GameManager>();
        _currentObjectCandiate = null;
        _isActive = true;
    }

    /// <summary>
    /// Sets the selection mode to active, hence a selection is taking place.
    /// </summary>
    public void SetSelectionModeActive()
    {
        _isActive = true;
    }

    /// <summary>
    /// Sets the selection mode to inactive, hence no selection is taking place.
    /// </summary>
    public void SetSelectionModeInactive()
    {
        _isActive = false;
    }

    /// <summary>
    /// Within every frame, the controller checks if a selection is taking place.
    /// If it is active, it gets the object under the mouse and references it.
    /// </summary>
    void Update()
    {
        if (_isActive)
        {
            CurrentObjectUnderMouse();
            SelectWhenClicked();
        }
    }

    /// <summary>
    /// Checks if an object has been selected.
    /// left mouse click - selects the object when there is one at the position
    /// right mouse click - deselects the object
    /// </summary>
    private void SelectWhenClicked()
    {
        if (Input.GetMouseButtonDown(0) && _currentObjectCandiate != null)
        {
            _gc.SetSelectedObject(_currentObjectCandiate);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            _gc.SetSelectedObject(null);
        }
    }

    /// <summary>
    /// Gets the current object under the mouse position.
    /// </summary>
    private void CurrentObjectUnderMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        var layerMask = LayerMask.GetMask("Tower", "Enemy");
        Physics.Raycast(ray, out hitInfo, Mathf.Infinity, layerMask);
        _currentObjectCandiate = hitInfo.transform;
    }
}