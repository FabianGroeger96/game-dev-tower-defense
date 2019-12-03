using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class ProjectileSpawner : MonoBehaviour
{
    private Projectile _projectile;
    private float _damage;
    private Dictionary<string, float> _projectileProperties;


    public void SetProjectileProperties(Dictionary<string, float> properties)
    {
        _projectileProperties = properties;
    }

    public void SetProjectile(Projectile projectile)
    {
        _projectile = projectile;
    }

    public void Fire(Transform target)
    {
        Projectile projectile = Instantiate(_projectile, transform.position, Quaternion.identity);
        projectile.SetTarget(target);
        projectile.SetProjectileDamage(_damage);
        projectile.SetProjectileProperties(_projectileProperties);
        projectile.Launch();
    }

    public void SetProjectileDamage(float damage)
    {
        _damage = damage;
    }
}
