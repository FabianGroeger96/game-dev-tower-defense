using System;
using UnityEditor;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class SelectionController : MonoBehaviour
{
    private GameController _gc;

    private bool _isActive;
    private Transform _currentObjectCandiate;
    
    void Start()
    {
        _gc = GetComponent<GameController>();
        _currentObjectCandiate = null;
        _isActive = true;
    }

    public void SetSelectionModeActive()
    {
        _isActive = true; 
    }
    
    public void SetSelectionModeInactive()
    {
        _isActive = false;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (_isActive)
        {
            CurrentObjectUnderMouse();
            SelectWhenClicked();
        }
    }
    
    private void SelectWhenClicked()
    {
        if (Input.GetMouseButtonDown(0) && _currentObjectCandiate != null)
        {
            _gc.SetSelectedObject(_currentObjectCandiate);
        }
    }
    
    private void CurrentObjectUnderMouse()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        var layerMask = LayerMask.GetMask("Tower", "Enemy");
        Physics.Raycast(ray, out hitInfo, Mathf.Infinity, layerMask);
        _currentObjectCandiate = hitInfo.transform;
    }
    
}
