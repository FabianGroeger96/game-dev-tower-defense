using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ProjectileLaser : Projectile
{
    private LineRenderer _lr;
    private Enemy _enemy;
    private bool _launched;
    public GameObject damageEffect;
    
    void LateUpdate()
    {
        if (_launched && target != null)
        {
            _lr.SetPosition(1, target.position);
        }
    }
    
    public override void Launch()
    {
        _lr = GetComponent<LineRenderer>();
        _lr.SetPosition(0, transform.position);
        _lr.SetPosition(1, target.position);
        var effect = Instantiate(damageEffect, target.position, Quaternion.identity);
        Destroy(effect, 3f);
        _enemy = target.gameObject.GetComponentInParent<Enemy>();
        _launched = true;
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.5f);
        if (_enemy != null)
        {
            _enemy.DealDamage(damage);
        }

        _launched = false;
        Destroy(gameObject);
    }
}
