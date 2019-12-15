using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Enemy : BaseController
{
    public int initialHealth;
    public int earning;
    public float speed;
    public int damage;
    public float health;
    
    private int _waypointIndex;
    private Transform _target;

    public GameObject deathEffect;

    [Header("Unity UI")] public Image healthBar;
    
    // Start is called before the first frame update
    void Start()
    {
        Init();
        health = initialHealth;
        _waypointIndex = 0;
        _target = Waypoints.waypoints[_waypointIndex];
        _waypointIndex++;
    }

    // Update is called once per frame
    void Update()
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
    }

    public void DealDamage(float damage)
    {
        health -= damage;
        healthBar.fillAmount = health / initialHealth;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 5f);

        GameController.enemiesAlive--;
        _gc.RegisterKill(earning);
        Destroy(gameObject);
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
}