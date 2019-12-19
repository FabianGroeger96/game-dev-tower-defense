using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

/// <summary>
/// Represents a tower in the game.
/// It is an attackable object, which has his own health and can take damage.
/// </summary>
public class Tower : AttackableObject
{
    [SerializeField] private Material _placeableMaterial;
    // reference to the unplaceable material
    [SerializeField] private Material _unplaceableMaterial;
    // reference to the placed material
    [SerializeField] private Material _placedMaterial;
    
    // array to set projectile damage corresponding to projectile
    [SerializeField] private float[] _projectileDamages;
    // array of possible projectiles
    [SerializeField] private Projectile[] _projectiles;
    // array of projetile shootpace corresponding to projectile
    [SerializeField] private float[] _projectilesShootPaces;
    // projectile base speed - only one value as physics is used to shoot
    [SerializeField] private float _projectileSpeed;
    
    // multiplier of the shootpace, used to power up the tower
    private float _shootPaceMultiplier;
    // multiplier of the damage, used to power up the tower
    private float _damageMultiplier;
    
    // if tower is in original projectile mode
    private bool _orginalProjectileMode;
    // current projectile
    private Projectile _currentProjectile;
    // current projectile damage
    private float _currentProjectileDamage;
    // current shoot pace
    private float _currentShootPace;

    [SerializeField] public int costs;
    // name of the tower
    [SerializeField] public string name;
    // upgrade cost of the tower
    [NonSerialized] public float upgradeCost;
    // level of the tower
    [NonSerialized] public int level;
    // sell value of the tower
    [NonSerialized] public float sellValue;
    
    // reference to the projectile spawner, to fire projectiles
    private ProjectileSpawner _spawner;
    // reference to the target finder, to find the enemy to shoot at
    private TargetFinder _targetFinder;
    // reference to the material controller, to change the material of the tower
    private ObjectMaterialController _omc;
    // in which direction the tower looks
    private Transform _rotation;
    // the effect that will be played when upgraded
    public GameObject upgradeEffect;
    
    // reference to the list of transforms
    private Transform[] _transforms;
    
    // timer to keep track of the shooting rate
    private float _timer;
    
    // if the tower has been placed
    private bool _placed;
    // if the tower is placeable
    private bool _placeable;
    // if the tower is colliding
    private bool _collids;
    
    /// <summary>
    /// Awake is being used to initialize all the reference the class needs,
    /// and to bring it to an initial state.
    /// </summary>
    void Awake()
    {
        _timer = 0f;
        _placed = false;
        _orginalProjectileMode = true;
        _currentProjectile = _projectiles[0];
        _currentProjectileDamage = _projectileDamages[0];
        _currentShootPace = _projectilesShootPaces[0];
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
        _spawner.SetProjectileSpeed(_projectileSpeed);
        _spawner.SetProjectileDamage(_currentProjectileDamage);
        _spawner.SetProjectile(_currentProjectile);
        SetLayerToPlaceMode();
    }
    
    /// <summary>
    /// Within every frame it checks if the tower is placed or not.
    /// If the tower is placed the tower acts according to the state of the game.
    /// If the tower is not placed yet, it checks if it is able to be placed at the point selected.
    /// </summary>
    void Update()
    {
        base.Update();
        if (_placed)
        {
            Act();
        }
        else
        {
            if (_placeable && _collids)
            {
                IsUnplaceable();
            }
        }
    }
    
    /// <summary>
    /// Event that fires when the collision stays with some other object.
    /// Used to check if the tower is able to be placed at the point selected.
    /// </summary>
    /// <param name="other">object it collides with</param>
    private void OnCollisionStay(Collision other)
    {
        _collids = true;
    }
    
    /// <summary>
    /// Event that fires when the collision enters with some other object.
    /// Used to check if the tower is able to be placed at the point selected.
    /// </summary>
    /// <param name="other">object it collides with</param>
    private void OnCollisionEnter(Collision other)
    {
        _collids = true;
    }
    
    /// <summary>
    /// Event that fires when the collision exits with some other object.
    /// Used to check if the tower is able to be placed at the point selected.
    /// </summary>
    /// <param name="other">object it collides with</param>
    private void OnCollisionExit(Collision other)
    {
        _collids = false;
    }
    
    /// <summary>
    /// Checks if a target is found to attack and then rotates to it and fires the projectile.
    /// </summary>
    private void Act()
    {
        if (_targetFinder.target != null)
        {
            RotateToTarget();
            _timer += Time.deltaTime;
            if (_timer > (_currentShootPace * _shootPaceMultiplier))
            {
                _spawner.Fire(_targetFinder.target.transform);
                _timer = 0f;
            }
        }
    }
    
    /// <summary>
    /// Sets the layer to placement mode.
    /// </summary>
    private void SetLayerToPlaceMode()
    {
        transform.gameObject.layer = LayerMask.NameToLayer("PlaceMode");
        foreach (Transform child in _transforms)
        {
            child.gameObject.layer = LayerMask.NameToLayer("PlaceMode");
        }
    }
    
    /// <summary>
    /// Sets the layer to tower mode.
    /// </summary>
    private void SetLayerToTower()
    {
        transform.gameObject.layer = LayerMask.NameToLayer("Tower");
        foreach (Transform child in _transforms)
        {
            child.gameObject.layer = LayerMask.NameToLayer("Tower");
        }
    }
    
    /// <summary>
    /// Tower rotates to the target found, to then fire a bullet in that direction.
    /// </summary>
    private void RotateToTarget()
    {
        Vector3 direction = _targetFinder.target.transform.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        Vector3 rotation = Quaternion.Lerp(_rotation.rotation, lookRotation, Time.deltaTime * 10).eulerAngles;
        _rotation.rotation = Quaternion.Euler(0f, rotation.y, 0f);
    }
    
    /// <summary>
    /// Checks if the tower is placeable and changes the material accordingly.
    /// </summary>
    public void IsPlaceable()
    {
        if (!_placeable)
        {
            _placeable = true;
            _omc.ChangeMaterial(_placeableMaterial);
        }
    }
    
    /// <summary>
    /// Checks if the tower is unplaceable and changes the material accordingly.
    /// </summary>
    public void IsUnplaceable()
    {
        if (_placeable)
        {
            _placeable = false;
            _omc.ChangeMaterial(_unplaceableMaterial);
        }
    }
    
    /// <summary>
    /// Places the tower at the position of the cursor.
    /// </summary>
    public void Place()
    {
        SetLayerToTower();
        _omc.ChangeMaterial(_placedMaterial);
        _placed = true;
    }
    
    /// <summary>
    /// Gets the placeable state.
    /// </summary>
    /// <returns>placeable state</returns>
    public bool GetPlaceableState()
    {
        return _placeable;
    }
    
    /// <summary>
    /// Gets the placed state.
    /// </summary>
    /// <returns>placed state</returns>
    public bool GetPlacedState()
    {
        return _placed;
    }
    
    /// <summary>
    /// Changes the target finder mode to the given one.
    /// </summary>
    /// <param name="mode">given target finder mode</param>
    public void ChangeTargetFinderMode(TargetFinder.TargetFinderMode mode)
    {
        _targetFinder.ChangeMode(mode);
    }
    
    /// <summary>
    /// Returns the current target finder mode.
    /// </summary>
    /// <returns>target finder mode</returns>
    public TargetFinder.TargetFinderMode GetTargetFinderMode()
    {
        return _targetFinder.GetMode();
    }
    
    /// <summary>
    /// Upgrades the current tower.
    /// </summary>
    public void UpgradeTower()
    {
        initialHealth += initialHealth / 2;
        health = initialHealth;
        level += 1;
        sellValue += upgradeCost / 2;
        upgradeCost += upgradeCost;
        _damageMultiplier += 0.25f;
        _shootPaceMultiplier -= 0.15f;
        _spawner.SetProjectileDamage(_currentProjectileDamage * _damageMultiplier);
    }

    /// <summary>
    /// Changes the projectile of the tower
    /// </summary>
    public void ChangeProjectileMode()
    {
        if (_projectiles.Length == 1)
        {
            return;
        }
        
        if (_orginalProjectileMode)
        {
            _currentProjectile = _projectiles[1];
            _currentProjectileDamage = _projectileDamages[1];
        }
        else
        {
            _currentProjectile = _projectiles[0];
            _currentProjectileDamage = _projectileDamages[0];
        }
        _spawner.SetProjectile(_currentProjectile);
        _spawner.SetProjectileDamage(_currentProjectileDamage);
        _orginalProjectileMode = !_orginalProjectileMode;
        
    }
    
    /// <summary>
    /// Method that will be called when the current tower dies, it plays a death effect and then destroys the object.
    /// </summary>
    protected override void Die()
    {
        GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 5f);

        Destroy(gameObject);
    }
}
