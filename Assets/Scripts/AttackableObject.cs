using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AttackableObject : MonoBehaviour
{
    public int initialHealth;
    public float health;
    
    public void DealDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    abstract protected void Die();
    
}
