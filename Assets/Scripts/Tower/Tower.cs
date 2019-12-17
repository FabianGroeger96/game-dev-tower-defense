using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;


public class Tower : AttackableObject
{
    [SerializeField] private Projectile _projectile;
    [SerializeField] private string[] _projectilePropertiesKeys;
    [SerializeField] private float[] _projectilePropertiesValues;
    [SerializeField] private float _damage;
    [SerializeField] private Material _placeableMaterial;
    [SerializeField] private Material _unplaceableMaterial;
    [SerializeField] private Material _placedMaterial;
    
    [SerializeField] private float _shootPace;
    
    private float _shootPaceMultiplier;
    private float _damageMultiplier;
    
    [SerializeField] public int costs;
    [SerializeField] public string name;
    [NonSerialized] public float upgradeCost;
    [NonSerialized] public int level;
    [NonSerialized] public float sellValue;
    
    private ProjectileSpawner _spawner;
    private TargetFinder _targetFinder;
    private ObjectMaterialController _omc;
    private Transform _rotation;
    public GameObject upgradeEffect;
    
    private Transform[] _transforms;
    
    private float _timer;
    
    private bool _placed;
    private bool _placeable;
    private bool _collids;
    
    void Awake()
    {
        _timer = 0f;
        _placed = false;
        
        _spawner = GetComponentInChildren<ProjectileSpawner>();
        _targetFinder = GetComponentInChildren<TargetFinder>();
        _omc = GetComponent<ObjectMaterialController>();
        _rotation = GetComponentInChildren<Transform>();
        _transforms = GetComponentsInChildren<Transform>();
        
        _damageMultiplier = 1f;
        _shootPaceMultiplier = 1f;
        _collids = false;
        
        level = 1;
        sellValue = costs / 2;
        upgradeCost = costs / 2;

        health = initialHealth;
        
        _omc.SetBaseMaterial(_placedMaterial);
        
        _spawner.SetProjectileDamage(_damage);
        _spawner.SetProjectile(_projectile);
        _spawner.SetProjectileProperties(CreateProjectilePropertiesDictionary());
        SetLayerToPlaceMode();
    }

    private Dictionary<string, float> CreateProjectilePropertiesDictionary()
    {
        Dictionary<string, float> dict = new Dictionary<string, float>();
        int i = 0;
        foreach (float value in _projectilePropertiesValues)
        {
            dict.Add(_projectilePropertiesKeys[i], value);
            i++;
        }
        return dict; 
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
            _timer += Time.deltaTime;
            if(_timer > (_shootPace * _shootPaceMultiplier)){
                _spawner.SetProjectileProperties(CreateProjectilePropertiesDictionary());
                _spawner.Fire(_targetFinder.target.transform);
                _timer = 0f;
            }
        }
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
            child.gameObject.layer = LayerMask.NameToLayer("Tower");
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
        if (!_placeable)
        {
            _placeable = true;
            _omc.ChangeMaterial(_placeableMaterial);
        }
    }
    public void IsUnplaceable()
    {
        if (_placeable)
        {
            _placeable = false;
            _omc.ChangeMaterial(_unplaceableMaterial);
        }
    }
    
    public void Place()
    {
        SetLayerToTower();
        _omc.ChangeMaterial(_placedMaterial);
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
    public void ChangeTargetFinderMode(TargetFinder.TargetFinderMode mode)
    {
        _targetFinder.ChangeMode(mode);
    }
    public TargetFinder.TargetFinderMode getTargetFinderMode()
    {
        return _targetFinder.GetMode();
    }
    public void UpgradeTower()
    {
        initialHealth += initialHealth / 2;
        health = initialHealth;
        level += 1;
        sellValue += upgradeCost / 2;
        upgradeCost += upgradeCost;
        _damageMultiplier += 0.25f;
        _shootPaceMultiplier -= 0.15f;
        _spawner.SetProjectileDamage(_damage * _damageMultiplier);
    }
    protected override void Die()
    {
        throw new System.NotImplementedException();
    }
}
