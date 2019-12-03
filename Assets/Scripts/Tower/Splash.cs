using UnityEngine;
using System.Collections.Generic;

public class Splash : MonoBehaviour
{
    public int damagePerParticle;
    public int hits;
    
    void Start()
    {
        
    }

    void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Enemy"))
        {
            hits++;
            Debug.Log(hits);
            Enemy enemy = other.GetComponentInParent<Enemy>();
            enemy.DealDamage(damagePerParticle);
        }
    }
}