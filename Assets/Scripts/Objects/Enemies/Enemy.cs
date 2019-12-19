using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

/// <summary>
/// Represents an enemy in the game,
/// which inherits from an attackable object.
/// </summary>
public class Enemy : AttackableObject
{
    // game variables to balance the enemy
    public int earning;
    public float speed;
    public int damage;
    public int level;
    // name of the enemy
    public string name;
    // where the enemy will start
    public int startingWaypoint = 1;
    // the material of the enemy
    public Material enemyMaterial;
    
    // what the next waypont index is
    private int _waypointIndex;
    // what the next target is
    private Transform _target;
    // reference to the GameManager to check the current status of the game
    private GameManager _gc;
    // reference to the ObjectMaterialController to change the material of the enemy for a given state
    private ObjectMaterialController _omc;

    // if the enemy has been killed yet
    private bool _killed = false;
    
    /// <summary>
    /// Awake is being used to initialize all the reference the class needs,
    /// and to bring it to an initial state.
    /// </summary>
    protected void Awake()
    {
        _omc = gameObject.GetComponent<ObjectMaterialController>();
        _gc = GameObject.Find("GameManager").GetComponent<GameManager>();
        CalculateMultipliesAccordingToLevel();
        ResetEnemy();
        _omc.SetBaseMaterial(enemyMaterial);
        _waypointIndex = startingWaypoint - 1;
        _target = Waypoints.waypoints[_waypointIndex];
        _waypointIndex++;
    }
    
    /// <summary>
    /// Calculates the earning, damage and initialHealth according to the level of the enemy,
    /// because of this an enemy can be harder to kill if he has a high level.
    /// </summary>
    private void CalculateMultipliesAccordingToLevel()
    {
        earning *= level;
        damage *= level;
        initialHealth *= level;
    }

    /// <summary>
    /// Within every frame, it checks if the game is still running and then calculates where the enemy has to move
    /// to get to the next way point, when the final way point is reached the enemy will be destroyed.
    /// </summary>
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

    /// <summary>
    /// Deals damage to the enemy, and kills it if the health is zero.
    /// </summary>
    /// <param name="damage">Damage to subtract from the enemy</param>
    public new void DealDamage(float damage)
    {
        SoundController.PlayEnemyHit();
        base.DealDamage(damage);
    }
    
    /// <summary>
    /// Will be called when the health of the enemy reaches zero, it specifies what to do when an enemy is killed.
    /// </summary>
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
    
    /// <summary>
    /// Will be called when the end point of all way points is reached.
    /// </summary>
    private void EndPointReached()
    {
        GameManager.enemiesAlive--;
        _gc.ownBase.DealDamage(damage);
        Destroy(gameObject);
    }
    
    /// <summary>
    /// Gets the next way point of all the way points.
    /// </summary>
    private void NextWaypoint()
    {
        _target = Waypoints.waypoints[_waypointIndex];
        _waypointIndex++;
    }
    
    /// <summary>
    /// Sets the new level of the enemy.
    /// </summary>
    /// <param name="newLevel">new level of enemy</param>
    public void SetLevel(int newLevel)
    {
        level = newLevel;
        CalculateMultipliesAccordingToLevel();
        ResetEnemy();
    }

    public void ResetEnemy()
    {
        health = initialHealth;
    }
}