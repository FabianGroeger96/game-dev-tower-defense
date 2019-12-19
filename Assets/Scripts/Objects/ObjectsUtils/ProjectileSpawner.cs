using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the spawner of a projectile, used to fire projectiles at targets.
/// </summary>
public class ProjectileSpawner : MonoBehaviour
{
    // reference to the projectile it has to spawn
    private Projectile _projectile;

    // how much damage the projectile to spawn does
    private float _damage;
    
    // how fast is the projectile when spawned
    private float _speed;
    
    /// <summary>
    /// Sets the projectile to spawn to the given one.
    /// </summary>
    /// <param name="projectile">projectile to spawn</param>
    public void SetProjectile(Projectile projectile)
    {
        _projectile = projectile;
    }

    /// <summary>
    /// Fires a specified projectile to hit a given target.
    /// </summary>
    /// <param name="target">the target the projectile has to hit</param>
    public void Fire(Transform target)
    {
        var projectile = Instantiate(_projectile, transform.position, Quaternion.identity);
        projectile.SetTarget(target);
        projectile.SetProjectileSpeed(_speed);
        projectile.SetProjectileDamage(_damage);
        projectile.Launch();
    }

    /// <summary>
    /// Sets the damage of the projectile to the given one.
    /// </summary>
    /// <param name="damage">damage of the projectile</param>
    public void SetProjectileDamage(float damage)
    {
        _damage = damage;
    }

    /// <summary>
    /// Sets the speed of the projectile.
    /// </summary>
    /// <param name="speed">speed of the projectile</param>
    public void SetProjectileSpeed(float projectileSpeed)
    {
        _speed = projectileSpeed;
    }
}