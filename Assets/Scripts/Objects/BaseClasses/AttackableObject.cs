using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Represents an object in the game which is attackable.
/// </summary>
public abstract class AttackableObject : MonoBehaviour
{
    // the initial health of the object
    public int initialHealth;
    // the current health of the object
    public float health;
    
    [Header("Effects")]
    // the effect it plays when the object dies
    public GameObject deathEffect;
    
    [Header("Health UI")] 
    // the canvas of the health bar to hide / show it
    public GameObject healthCanvas;
    // the image of the health bar to control its fill amount
    public Image healthBar;
    
    /// <summary>
    /// Deals damage to the attackable object, it subtracts the given damage from the current health,
    /// controls the amount to fill of the health bar and checks if the health is below zero.
    /// </summary>
    /// <param name="damage">the damage to subtract from the object</param>
    public void DealDamage(float damage)
    {
        health -= damage;
        healthBar.fillAmount = health / initialHealth;
        if (health <= 0)
        {
            Die();
        }
    }
    
    /// <summary>
    /// Within every frame it shows the health bar according to the UIController.
    /// </summary>
    public void Update()
    {
        healthCanvas.SetActive(UIController.showHealthBars);
    }
    
    /// <summary>
    /// Abstract method that which will represent the death of an object.
    /// </summary>
    protected abstract void Die();
}
