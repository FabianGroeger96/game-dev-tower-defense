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
    private Tower _currentPlaceableObject;
    private bool _validPlace;
    
    void Start()
    {
        _gc = GetComponent<GameController>();
        _isActive = false;
        _validPlace = false;
    }

    public void SetPlacementModeInactive()
    {
        if (_currentPlaceableObject.GetPlacedState() == false)
        {
            DestroyCurrentPlaceableObject();
        }
        _isActive = false; 
    }

    public void DestroyCurrentPlaceableObject()
    {
        Destroy(_currentPlaceableObject.gameObject);
        _currentPlaceableObject = null;
    }
    
    public void SetPlacementModeActive(Tower placedTower)
    {
        _isActive = true;
        _currentPlaceableObject = Instantiate(placedTower);
        _currentPlaceableObject.IsPlaceable();
    }
    
    // Update is called once per frame
    void Update()
    {
        if (_isActive && _currentPlaceableObject != null)
        {
            MoveCurrentObjectToMouse();
            //RotateFromMouseWheel();
            ReleaseIfClicked();
        }
    }
    
    private void ReleaseIfClicked()
    {
        if (Input.GetMouseButtonUp(0) && _currentPlaceableObject.GetPlaceableState())
        {
            _currentPlaceableObject.Place();
            _gc.TowerPlaced(_currentPlaceableObject.costs);
        }
    }
    
    private void MoveCurrentObjectToMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        var layerMask = LayerMask.GetMask("Ground", "Path", "Tower", "SeenGround");
        Physics.Raycast(ray, out hitInfo, Mathf.Infinity, layerMask);
        if (hitInfo.transform != null)
        {
            _currentPlaceableObject.transform.localPosition = hitInfo.point;
            _currentPlaceableObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
            if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer(_placementLayer))
            {
                _currentPlaceableObject.IsPlaceable();
            }
            else
            {
                _currentPlaceableObject.IsUnplaceable();
            }
        }
    }
    
}
