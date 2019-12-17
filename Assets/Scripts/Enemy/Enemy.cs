﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Enemy : AttackableObject
{
    public int earning;
    public float speed;
    public int damage;
    public int level;
    public string name;
    public int startingWaypoint = 1;
    
    private int _waypointIndex;
    private Transform _target;
    private GameController _gc;
    private ObjectMaterialController _omc;
    public Material enemyMaterial;
    
    private bool killed = false;

    public GameObject deathEffect;

    [Header("Unity UI")] public GameObject healthCanvas;
    public Image healthBar;
    
    // Start is called before the first frame update
    protected void Awake()
    {
        _omc = gameObject.GetComponent<ObjectMaterialController>();
        _gc = GameObject.Find("GameController").GetComponent<GameController>();
        CalculateMultipliesAccordingToLevel();
        _omc.SetBaseMaterial(enemyMaterial);
        health = initialHealth;
        _waypointIndex = startingWaypoint - 1;
        _target = Waypoints.waypoints[_waypointIndex];
        _waypointIndex++;
    }

    private void CalculateMultipliesAccordingToLevel()
    {
        earning *= level;
        damage *= level;
        initialHealth *= level;
    }

    // Update is called once per frame
    protected void Update()
    {
        if (_gc.gameState == GameController.GameState.Running)
        {
            Vector3 dir = Vector3.Scale(new Vector3(1, 0, 1), _target.position - transform.position);
            transform.Translate(dir.normalized * speed * Time.deltaTime, Space.World);
            Vector2 position2d = new Vector2(transform.position.x, transform.position.z);
            Vector2 target2d = new Vector2(_target.transform.position.x, _target.transform.position.z);
            if (Vector2.Distance(position2d, target2d) <= 0.1f)
            {
                if (_waypointIndex >= Waypoints.waypoints.Length)
                {
                    EndPointReached();
                }
                else
                {
                    NextWaypoint();
                }
                
            }
        }

        if (UIController.showHealthBars)
        {
            healthCanvas.SetActive(true);
        }
        else
        {
            healthCanvas.SetActive(false);
        }
    }

    public void DealDamage(float damage)
    {
        base.DealDamage(damage);
        healthBar.fillAmount = health / initialHealth;
    }
    
    protected override void Die()
    {
        GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 5f);

        if (!killed)
        {
            GameController.enemiesAlive--;
            GameController.enemiesKilled++;
            _gc.RegisterKill(earning);
            Destroy(gameObject);
            killed = true;
        }
    }

    private void EndPointReached()
    {
        GameController.enemiesAlive--;
        _gc.RemoveLifeCount(damage);
        Destroy(gameObject);
    }

    private void NextWaypoint()
    {
        _target = Waypoints.waypoints[_waypointIndex];
        _waypointIndex++;
    }

    public void SetLevel(int newLevel)
    {
        level = newLevel;
        CalculateMultipliesAccordingToLevel();
    }
}