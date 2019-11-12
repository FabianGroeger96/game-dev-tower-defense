using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class PlacementController : MonoBehaviour
{
    private Tower _tower;
    private Tower[] _placeableObjects;
    private int _currentPlaceableObjectNumber;
    public int layerMask;
    public Tower currentPlaceableObject;
    
    // Start is called before the first frame update
    void Start()
    {
        _placeableObjects = new Tower[10];
        layerMask = 1 << 11;
        _tower = (Tower) AssetDatabase.LoadAssetAtPath("Assets/Prefabs/RocketSilo.prefab", typeof(Tower));
        _placeableObjects[0] = (Tower) AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Tower.prefab", typeof(Tower));
        _placeableObjects[1] = (Tower) AssetDatabase.LoadAssetAtPath("Assets/Prefabs/RocketSilo.prefab", typeof(Tower));
    }
    
    public void placeMode(int placedObject)
    {
        if (currentPlaceableObject != null && _currentPlaceableObjectNumber == placedObject)
        {
            Destroy(currentPlaceableObject.gameObject);
        }
        else if (currentPlaceableObject != null)
        {
            Destroy(currentPlaceableObject.gameObject);
            currentPlaceableObject = Instantiate(_placeableObjects[placedObject - 1]);
            _currentPlaceableObjectNumber = placedObject;

        }
        else
        {
            currentPlaceableObject = Instantiate(_placeableObjects[placedObject - 1]);
            _currentPlaceableObjectNumber = placedObject;
        }
        
    }
    
    // Update is called once per frame
    void Update()
    {
        //HandleNewObjectHotkey();

        if (currentPlaceableObject != null)
        {
            MoveCurrentObjectToMouse();
            //RotateFromMouseWheel();
            ReleaseIfClicked();
        }
        


    }

    private void ReleaseIfClicked()
    {
        if (Input.GetMouseButtonUp(0))
        {
            currentPlaceableObject = null;
        }
    }

    private void MoveCurrentObjectToMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo, Mathf.Infinity, layerMask) && hitInfo.transform.name == "Ground")
        {
            currentPlaceableObject.transform.localPosition = hitInfo.point;
            currentPlaceableObject.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
        }
    }

    private void HandleNewObjectHotkey()
    {
        if (Input.GetKeyUp(KeyCode.A))
        {
            
            if (currentPlaceableObject != null)
            {
                Destroy(currentPlaceableObject.gameObject);
                
            }
            else
            {
                currentPlaceableObject = Instantiate(_tower);
            }
        }
    }
}
