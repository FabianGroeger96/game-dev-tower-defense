using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

/// <summary>
/// Represents a shooting enemy in the game, which can damage the towers.
/// It inherits from enemy.
/// </summary>
public class ShootingEnemy : Enemy
{
    // game variables to balance the shooting enemy and specifies how much damage it makes
    [SerializeField] private float _damage;
    [SerializeField] private float _shootPace;
    [SerializeField] private float _shootPaceMultiplier;
    
    // the effect to play when it damages a tower
    public GameObject damageEffect;
    
    // to find the next target to attack
    private TargetFinder _targetFinder;
    // to render the line (laser)
    private LineRenderer _lr;
    // represents the tower to attack
    private Tower _tower;
    // specifies if the laser has been fired yet
    private bool _launched;
    // specifies if the laser can shoot again
    private float _timer;
    
    /// <summary>
    /// Awake is being used to initialize all the reference the class needs,
    /// and to bring it to an initial state.
    /// </summary>
    void Awake()
    {
        base.Awake();
        _timer = 0f;
        
        _targetFinder = GetComponentInChildren<TargetFinder>();
        _targetFinder.ChangeMode(TargetFinder.TargetFinderMode.NearestEnemy);
        _lr = GetComponentInChildren<LineRenderer>();
    }
    
    /// <summary>
    /// Within every frame, it checks if the enemy can attack a target.
    /// </summary>
    protected void Update()
    {
        base.Update();
        act();
        if (_launched)
        {
            _lr.SetPosition(0, transform.position);
        }
    }
    
    /// <summary>
    /// Checks if a target to damage is available.
    /// </summary>
    private void act()
    {
        if (_targetFinder.target != null)
        {
            _timer += Time.deltaTime;
            if(_timer > (_shootPace * _shootPaceMultiplier) &&
               _targetFinder.target.layer == LayerMask.NameToLayer("Tower"))
            {
                Shoot();
                _timer = 0f;
            }
        }
    }
    
    /// <summary>
    /// Shoots at a target.
    /// </summary>
    private void Shoot()
    {
        _lr = GetComponent<LineRenderer>();
        _lr.SetPosition(0, transform.position);
        _lr.SetPosition(1, _targetFinder.target.transform.position);
        var effect = Instantiate(damageEffect, _targetFinder.target.transform.position, Quaternion.identity);
        Destroy(effect, 3f);
        _tower = _targetFinder.target.GetComponentInParent<Tower>();
        _launched = true;
        StartCoroutine(Wait());
    }
    
    /// <summary>
    /// Waits for 0.5f seconds and then deals damage to the tower.
    /// </summary>
    /// <returns>NONE</returns>
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.5f);
        if (_tower != null)
        {
            _tower.DealDamage(_damage);
        }
        _launched = false;
        _lr.SetPosition(1, transform.position);
    }
}
