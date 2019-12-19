using System.Collections;
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
    private GameManager _gc;
    private ObjectMaterialController _omc;
    public Material enemyMaterial;

    private bool _killed = false;

    // Start is called before the first frame update
    protected void Awake()
    {
        _omc = gameObject.GetComponent<ObjectMaterialController>();
        _gc = GameObject.Find("GameManager").GetComponent<GameManager>();
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
    protected new void Update()
    {
        base.Update();
        if (_gc.gameState == GameManager.GameState.Running)
        {
            var dir = Vector3.Scale(new Vector3(1, 0, 1), _target.position - transform.position);
            transform.Translate(dir.normalized * (speed * Time.deltaTime), Space.World);

            var position = transform.position;
            var position2d = new Vector2(position.x, position.z);

            var targetPosition = _target.transform.position;
            var target2d = new Vector2(targetPosition.x, targetPosition.z);

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

    public new void DealDamage(float damage)
    {
        SoundController.PlayEnemyHit();
        base.DealDamage(damage);
    }
    
    protected override void Die()
    {
        var effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 5f);

        if (!_killed)
        {
            GameManager.enemiesAlive--;
            GameManager.enemiesKilled++;
            _gc.RegisterKill(earning);
            Destroy(gameObject);
            _killed = true;
        }
    }

    private void EndPointReached()
    {
        GameManager.enemiesAlive--;
        _gc.ownBase.DealDamage(damage);
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