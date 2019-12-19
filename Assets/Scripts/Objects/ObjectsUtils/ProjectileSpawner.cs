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

    // the properties of the projectile
    private Dictionary<string, float> _projectileProperties;

    /// <summary>
    /// Sets the projectile properties to the given ones.
    /// </summary>
    /// <param name="properties">projectile properties</param>
    public void SetProjectileProperties(Dictionary<string, float> properties)
    {
        _projectileProperties = properties;
    }

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
        projectile.SetProjectileDamage(_damage);
        projectile.SetProjectileProperties(_projectileProperties);
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
}