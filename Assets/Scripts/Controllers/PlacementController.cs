using System;
using UnityEditor;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class PlacementController : MonoBehaviour
{
    private Tower[] _placeableObjects;
    private int _currentPlaceableObjectNumber;
    private String _placementLayer = "Ground";
    private Tower _currentPlaceableObject;
    private bool _validPlace;

    // Start is called before the first frame update
    void Start()
    {
        _placeableObjects = new Tower[10];
        _placeableObjects[0] = (Tower) AssetDatabase.LoadAssetAtPath("Assets/Prefabs/RocketSilo.prefab", typeof(Tower));
        _placeableObjects[1] = (Tower) AssetDatabase.LoadAssetAtPath("Assets/Prefabs/RocketRotation.prefab", typeof(Tower));
        _validPlace = false;
    }
    
    public void placeMode(int placedObject)
    {
        if (_currentPlaceableObject != null && _currentPlaceableObjectNumber == placedObject)
        {
            Destroy(_currentPlaceableObject.gameObject);
            _currentPlaceableObject.IsUnplaceable();
            _validPlace = false;
        }
        else if (_currentPlaceableObject != null)
        {
            Destroy(_currentPlaceableObject.gameObject);
            _currentPlaceableObject = Instantiate(_placeableObjects[placedObject - 1]);
            _currentPlaceableObjectNumber = placedObject;
            _currentPlaceableObject.IsPlaceable();
        }
        else
        {
            _currentPlaceableObject = Instantiate(_placeableObjects[placedObject - 1]);
            _currentPlaceableObjectNumber = placedObject;
            _currentPlaceableObject.IsPlaceable();
        }
        
    }
    
    // Update is called once per frame
    void Update()
    {
        //HandleNewObjectHotkey();

        if (_currentPlaceableObject != null)
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
            _currentPlaceableObject.IsPlaced();
            _currentPlaceableObject = null;
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
            if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer(_placementLayer) 
                && _currentPlaceableObject.GetCollisionState() == false)
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
