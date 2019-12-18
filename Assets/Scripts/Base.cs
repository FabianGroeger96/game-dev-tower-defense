using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Base : AttackableObject
{
    private GameController _gc;
    private bool _killed = false;

    public GameObject damageEffect;

    private void Awake()
    {
        health = initialHealth;
        _gc = GameObject.Find("GameController").GetComponent<GameController>();
    }

    public new void DealDamage(float damage)
    {
        // play damage effect
        var effect = Instantiate(damageEffect, transform.position, Quaternion.identity);
        Destroy(effect, 6f);
        // call base deal damage method
        base.DealDamage(damage);
    }

    protected override void Die()
    {
        // check if base isn't killed yet
        if (!_killed)
        {
            _killed = true;
            // play death effect
            var effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(effect, 6f);
            // destroy base
            Destroy(gameObject);
            // set game state to game over
            _gc.gameState = GameController.GameState.GameOver;
        }
    }
}
