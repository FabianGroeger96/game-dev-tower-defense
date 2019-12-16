using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class ShootingEnemy : Enemy
{
    
    [SerializeField] private Projectile _projectile;
    [SerializeField] private float _damage;
    [SerializeField] private float _shootPace;
    [SerializeField] private float _shootPaceMultiplier;

    private ProjectileSpawner _spawner;
    private TargetFinder _targetFinder;
    
    private float _timer;
    
    void Start()
    {
        base.Start();
        _timer = 0f;

        _spawner = GetComponentInChildren<ProjectileSpawner>();
        _targetFinder = GetComponentInChildren<TargetFinder>();
        
        _spawner.SetProjectileDamage(_damage);
        _spawner.SetProjectile(_projectile);
    }
    
    private void act()
    {
        if (_targetFinder.target != null)
        {
            _timer += Time.deltaTime;
            if(_timer > (_shootPace * _shootPaceMultiplier)){
                _spawner.Fire(_targetFinder.target.transform);
                _timer = 0f;
            }
        }
    }
    

    // Update is called once per frame
    protected void Update()
    {
        base.Update();
        act();
    }
}
