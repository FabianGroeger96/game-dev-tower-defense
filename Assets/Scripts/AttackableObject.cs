using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class AttackableObject : MonoBehaviour
{
    public int initialHealth;
    public float health;
    
    [Header("Effects")]
    public GameObject deathEffect;
    
    [Header("Health UI")] 
    public GameObject healthCanvas;
    public Image healthBar;
    
    public void DealDamage(float damage)
    {
        health -= damage;
        healthBar.fillAmount = health / initialHealth;
        if (health <= 0)
        {
            Die();
        }
    }

    public void Update()
    {
        if (UIController.showHealthBars)
        {
            healthCanvas.SetActive(true);
        }
        else
        {
            healthCanvas.SetActive(false);
        }
    }

    abstract protected void Die();
    
}
