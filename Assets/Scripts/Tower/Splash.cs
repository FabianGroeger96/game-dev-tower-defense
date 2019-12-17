using UnityEngine;
using System.Collections.Generic;

public class Splash : MonoBehaviour
{
    public float damagePerParticle;
    public int hits;

    void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            hits++;
            Enemy enemy = other.GetComponentInParent<Enemy>();
            enemy.DealDamage(damagePerParticle);
        }
    }
}