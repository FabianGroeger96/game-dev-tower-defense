using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;


public class Tower : MonoBehaviour
{
    [SerializeField] public Material[] materials;
    [SerializeField] public int costs;
    
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
        _placed = false;
        
        _spawner = GetComponentInChildren<ProjectileSpawner>();
        _targetFinder = GetComponentInChildren<TargetFinder>();
        _rotation = GetComponentInChildren<Transform>();
        
        _renderers = GetComponentsInChildren<MeshRenderer>();
        _transforms = GetComponentsInChildren<Transform>();
        
        _collids = false;
        IsUnplaceable();
        SetLayerToPlaceMode();

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

    private void OnCollisionStay(Collision other)
    {
        _collids = true;
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
                _spawner.launch(this, _targetFinder.target.transform);
            }
        }
    }

    public void HandleHit(int obj)
    {
        Debug.Log("Tower hitted!");
    }
    
    private void SetLayerToPlaceMode()
    {
        transform.gameObject.layer = LayerMask.NameToLayer("PlaceMode");
        foreach (Transform child in _transforms)
        {
            child.gameObject.layer = LayerMask.NameToLayer("PlaceMode");
        }
    }

    private void SetLayerToTower()
    {
        transform.gameObject.layer = LayerMask.NameToLayer("Tower");
        foreach (Transform child in _transforms)
        {
            ChangeMaterial(0);
            child.gameObject.layer = LayerMask.NameToLayer("Tower");
        }
    }
    
    private void ChangeMaterial(int material)
    {
        foreach(MeshRenderer r in _renderers)
        {
            r.material = materials[material];
        }
        
    }
    
    private void RotateToTarget()
    {
        Vector3 direction = _targetFinder.target.transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.Lerp(_rotation.rotation, lookRotation, Time.deltaTime * 10).eulerAngles;
        _rotation.rotation = Quaternion.Euler(0f, rotation.y, 0f);
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
    public void Place()
    {
        SetLayerToTower();
        _placed = true;
    }
    public bool GetPlaceableState()
    {
        return _placeable;
    }
    public bool GetPlacedState()
    {
        return _placed;
    }
    
}
