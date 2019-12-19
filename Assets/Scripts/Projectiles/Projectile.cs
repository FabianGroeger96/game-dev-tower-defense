using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a projectile in the game, which will be fired from a tower.
/// </summary>
public abstract class Projectile : MonoBehaviour
{
    // damage of the projectile when it hits
    protected float damage;
    
    // speed of the projectile
    protected float speed;

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
    /// Sets the speed of the projectile.
    /// </summary>
    /// <param name="speed">the properties of the projectile</param>
    public void SetProjectileSpeed(float speed)
    {
        this.speed = speed;
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