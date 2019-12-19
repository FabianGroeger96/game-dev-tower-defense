using System;
using UnityEditor;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

/// <summary>
/// Controller that is used to place a tower in the game.
/// </summary>
public class PlacementController : MonoBehaviour
{
    // name of the placement layer
    [SerializeField] private String _placementLayer;

    // reference to the GameManager
    private GameManager _gc;

    // if a placement is taking place (active)
    private bool _isActive;

    // reference to the current placed tower
    private Tower _currentPlacedTower;

    // if the placement position is valid or not
    private bool _validPlace;

    /// <summary>
    /// Awake is being used to initialize all the reference the class needs,
    /// and to bring it to an initial state.
    /// </summary>
    void Awake()
    {
        _gc = GetComponent<GameManager>();
        _isActive = false;
        _validPlace = false;
    }

    /// <summary>
    /// Changes the placement mode to inactive.
    /// </summary>
    public void SetPlacementModeInactive()
    {
        if (_currentPlacedTower.GetPlacedState() == false)
        {
            DestroyCurrentPlaceableObject();
        }

        _isActive = false;
    }

    /// <summary>
    /// Destroyed the current placeable object.
    /// </summary>
    public void DestroyCurrentPlaceableObject()
    {
        Destroy(_currentPlacedTower.gameObject);
        _currentPlacedTower = null;
    }

    /// <summary>
    /// Sets the placement mode to active and provides the tower to place on the ground.
    /// </summary>
    /// <param name="placedTower">tower to place</param>
    public void SetPlacementModeActive(Tower placedTower)
    {
        _isActive = true;
        _currentPlacedTower = Instantiate(placedTower);
        _currentPlacedTower.IsPlaceable();
    }

    /// <summary>
    /// Within every frame the controller checks if a placement takes place.
    /// If one is taking place, it moves the selected tower to the mouse pointer and releases it on mouse click.
    /// </summary>
    void Update()
    {
        if (_isActive && _currentPlacedTower != null)
        {
            MoveCurrentObjectToMouse();
            ReleaseIfClicked();
        }
    }

    /// <summary>
    /// Listens for the mouse down click, checks if the position is valid and then places the tower.
    /// </summary>
    private void ReleaseIfClicked()
    {
        if (Input.GetMouseButtonDown(0) && _currentPlacedTower.GetPlaceableState())
        {
            _currentPlacedTower.Place();
            _gc.TowerPlaced(_currentPlacedTower.costs);
        }
    }

    /// <summary>
    /// Moves the current tower to the mouse pointer.
    /// </summary>
    private void MoveCurrentObjectToMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        var layerMask = LayerMask.GetMask("Ground", "Path", "Tower", "SeenGround", "Base", "Obstacle");
        Physics.Raycast(ray, out hitInfo, Mathf.Infinity, layerMask);
        if (hitInfo.transform != null)
        {
            _currentPlacedTower.transform.localPosition = hitInfo.point;
            _currentPlacedTower.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer(_placementLayer))
            {
                _currentPlacedTower.IsPlaceable();
            }
            else
            {
                _currentPlacedTower.IsUnplaceable();
            }
        }
    }
}