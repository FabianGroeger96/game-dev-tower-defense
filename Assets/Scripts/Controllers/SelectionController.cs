using System;
using UnityEditor;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class SelectionController : MonoBehaviour
{
    private GameManager _gc;

    private bool _isActive;
    private Transform _currentObjectCandiate;

    void Awake()
    {
        _gc = GetComponent<GameManager>();
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
        else if (Input.GetMouseButtonDown(1))
        {
            _gc.SetSelectedObject(null);
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