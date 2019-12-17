using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;

public class ShootingEnemy : Enemy
{
    [SerializeField] private float _damage;
    [SerializeField] private float _shootPace;
    [SerializeField] private float _shootPaceMultiplier;
    
    private TargetFinder _targetFinder;
    private LineRenderer _lr;
    private Tower _tower;
    private bool _launched;
    
    private float _timer;
    
    void Awake()
    {
        base.Awake();
        _timer = 0f;
        
        _targetFinder = GetComponentInChildren<TargetFinder>();
        _lr = GetComponentInChildren<LineRenderer>();
    }
    
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

    private void Shoot()
    {
        _lr = GetComponent<LineRenderer>();
        _lr.SetPosition(0, transform.position);
        _lr.SetPosition(1, _targetFinder.target.transform.position);
        _tower = _targetFinder.target.GetComponentInParent<Tower>();
        _launched = true;
        StartCoroutine(Wait());
    }
    
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
    
    protected void Update()
    {
        base.Update();
        act();
        if (_launched)
        {
            _lr.SetPosition(0, transform.position);
        }
        
        
    }
}
