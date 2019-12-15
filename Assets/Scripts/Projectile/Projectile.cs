using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{

    protected float damage;
    protected Transform target;
    protected Dictionary<string, float> properties;

    public void SetTarget(Transform target)
    {
        this.target = target;
    }
    public void SetProjectileProperties(Dictionary<string, float> properties)
    {
        this.properties = properties;
    } 
    
    public void SetProjectileDamage(float damage)
    {
        this.damage = damage;
    }
    
    abstract public void Launch();
    
}
