using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a projectile in the game, which will be fired from a tower.
/// </summary>
public abstract class Projectile : MonoBehaviour
{
    // damage of the projectile when it hits
    protected float damage;

    // target the projectile tries to hit
    protected Transform target;

    // properties of the projectile
    protected Dictionary<string, float> properties;

    /// <summary>
    /// Sets the target of a projectile.
    /// </summary>
    /// <param name="target">the target the projectile tries to hit</param>
    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    /// <summary>
    /// Sets the properties of the projectile.
    /// </summary>
    /// <param name="properties">the properties of the projectile</param>
    public void SetProjectileProperties(Dictionary<string, float> properties)
    {
        this.properties = properties;
    }

    /// <summary>
    /// Sets the damage of the projectile.
    /// </summary>
    /// <param name="damage">the damage of the projectile</param>
    public void SetProjectileDamage(float damage)
    {
        this.damage = damage;
    }

    /// <summary>
    /// Abstract method that will be called to launch the projectile.
    /// </summary>
    public abstract void Launch();
}