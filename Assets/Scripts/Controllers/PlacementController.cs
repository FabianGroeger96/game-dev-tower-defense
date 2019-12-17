using System;
using UnityEditor;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class PlacementController : MonoBehaviour
{
    [SerializeField] private String _placementLayer;

    private GameController _gc;

    private bool _isActive;
    private Tower _currentPlacedTower;
    private bool _validPlace;
    
    void Start()
    {
        _gc = GetComponent<GameController>();
        _isActive = false;
        _validPlace = false;
    }

    public void SetPlacementModeInactive()
    {
        if (_currentPlacedTower.GetPlacedState() == false)
        {
            DestroyCurrentPlaceableObject();
        }
        _isActive = false; 
    }

    public void DestroyCurrentPlaceableObject()
    {
        Destroy(_currentPlacedTower.gameObject);
        _currentPlacedTower = null;
    }
    
    public void SetPlacementModeActive(Tower placedTower)
    {
        _isActive = true;
        _currentPlacedTower = Instantiate(placedTower);
        _currentPlacedTower.IsPlaceable();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (_isActive && _currentPlacedTower != null)
        {
            MoveCurrentObjectToMouse();
            ReleaseIfClicked();
        }
    }
    
    private void ReleaseIfClicked()
    {
        if (Input.GetMouseButtonDown(0) && _currentPlacedTower.GetPlaceableState())
        {
            _currentPlacedTower.Place();
            _gc.TowerPlaced(_currentPlacedTower.costs);
        }
    }
    
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
