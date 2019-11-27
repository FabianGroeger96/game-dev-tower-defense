using System;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Experimental.Animations;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;


public class Tower : MonoBehaviour
{
    [SerializeField] public int exp;
    [SerializeField] public Material[] materials;
    
    private ProjectileSpawner _spawner;
    private TargetFinder _targetFinder;
    private Transform _rotation;
    
    private MeshRenderer[] _renderers;
    private Transform[] _transforms;
    
    private bool _placed;
    private bool _placeable;
    private bool _collids;
    
    void Start()
    {
        _spawner = GetComponentInChildren<ProjectileSpawner>();
        _targetFinder = GetComponentInChildren<TargetFinder>();
        _rotation = GetComponentInChildren<Transform>();
        
        _renderers = GetComponentsInChildren<MeshRenderer>();
        _transforms = GetComponentsInChildren<Transform>();
        
        _placed = false;
        _collids = false;
        IsUnplaceable();
        
        transform.gameObject.layer = LayerMask.NameToLayer("PlaceMode");
        foreach (Transform child in _transforms)
        {
            child.gameObject.layer = LayerMask.NameToLayer("PlaceMode");
        }
    }
    
    void Update()
    {
        if (_placed)
        {
            act();
        }
        else
        {
            if (_placeable && _collids)
            {
                IsUnplaceable();
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        _collids = true;
    }
    
    private void OnCollisionExit(Collision other)
    {
        _collids = false;
    }

    private void act()
    {
        if (_targetFinder.target != null)
        {
            RotateToTarget();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _spawner.launch(this, _targetFinder.target.transform, _rotation.rotation);
            }
        }
    }

    private void RotateToTarget()
    {
        Vector3 direction = _targetFinder.target.transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.Lerp(_rotation.rotation, lookRotation, Time.deltaTime * 10).eulerAngles;
        _rotation.rotation = Quaternion.Euler(0f, rotation.y, 0f);

    }

    public void HandleHit(int obj)
    {
        exp += obj;
    }

    private void ChangeMaterial(int material)
    {
        foreach(MeshRenderer r in _renderers)
        {
            r.material = materials[material];
        }
        
    }

    public void IsPlaceable()
    {
        _placeable = true;
        ChangeMaterial(1);
    }

    public void IsUnplaceable()
    {
        _placeable = false;
        ChangeMaterial(2);
    }
    
    public void IsPlaced()
    {
        transform.gameObject.layer = LayerMask.NameToLayer("Tower");
        foreach (Transform child in _transforms)
        {
            ChangeMaterial(0);
            child.gameObject.layer = LayerMask.NameToLayer("Tower");
        }
        _placed = true;
    }

    public bool GetPlaceableState()
    {
        return _placeable;
    }

    public bool GetCollisionState()
    {
        return _collids;
    }
}
