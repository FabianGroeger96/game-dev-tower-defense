using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Represents the target finder, for finding a target in sight to a object.
/// </summary>
public class TargetFinder : MonoBehaviour
{
    /// <summary>
    /// Represents the mode which the target finder can have.
    /// </summary>
    public enum TargetFinderMode
    {
        NearestEnemy,
        FollowFirstEnemy,
        LowestHealth,
        HighestHealth
    };
    
    // Reference to the current target found
    public GameObject target;
    // Specifies the range of the target searching
    public float range = 15f;
    // Specifies the squared range of the target searching
    private float _rangeSquared;
    // the current target finder mode
    private TargetFinderMode _mode;
    
    // reference to all eligable target tags
    public string[] eligableTargetTags;

    // color of the gizmo
    public Color gizmoColor = Color.yellow;
    
    /// <summary>
    /// Draws the gizmo in the shape of a sphere.
    /// </summary>
    void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, range);
    }
    
    /// <summary>
    /// Searches for attackable targets within the range.
    /// </summary>
    /// <returns>found targets</returns>
    private GameObject[] FindEligableTargets()
    {
        ArrayList eligableTargets = new ArrayList();

        foreach (string s in eligableTargetTags)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(s);
            foreach (GameObject enemy in enemies)
            {
                eligableTargets.Add(enemy);
            }
        }

        return (GameObject[]) eligableTargets.ToArray(typeof(GameObject));
    }
    
    /// <summary>
    /// Awake is being used to initialize all the reference the class needs,
    /// and to bring it to an initial state.
    /// </summary>
    private void Awake()
    {
        _mode = TargetFinderMode.NearestEnemy;
        _rangeSquared = range * range;
        InvokeRepeating(nameof(UpdateTarget), 0f, 0.25f);
    }
    
    /// <summary>
    /// Updates the target of the class according to the global target finder mode.
    /// </summary>
    void UpdateTarget()
    {
        _rangeSquared = range * range;
        GameObject[] enemies = FindEligableTargets();
        if (_mode == TargetFinderMode.NearestEnemy)
        {
            SetTarget(FindNearestEnemyInRange(enemies));
        }

        if (_mode == TargetFinderMode.FollowFirstEnemy)
        {
            FollowFirstEnemy(enemies);
        }

        if (_mode == TargetFinderMode.LowestHealth)
        {
            FindEnemyWithLowestHealth(enemies);
        }

        if (_mode == TargetFinderMode.HighestHealth)
        {
            FindEnemyWithHighestHealth(enemies);
        }

        if (!target)
        {
            target = null;
        }
    }
    
    /// <summary>
    /// Checks if the current target is still within range.
    /// </summary>
    /// <returns>target within range</returns>
    private bool CurrentTargetStillInRange()
    {
        if (target != null)
        {
            Vector3 offsetToCurrentTarget = target.transform.position - transform.position;
            offsetToCurrentTarget.y = 0;
            var sqrLenToCurrentTarget = offsetToCurrentTarget.sqrMagnitude;

            if (sqrLenToCurrentTarget < _rangeSquared)
            {
                return true;
            }

            return false;
        }

        return false;
    }
    
    /// <summary>
    /// Finds Targets with the highest Health.
    /// </summary>
    /// <param name="enemies">list of enemies in sight</param>
    private void FindEnemyWithHighestHealth(GameObject[] enemies)
    {
        target = null;
        float _lowestHealth = -Mathf.Infinity;
        foreach (var enemy in enemies)
        {
            AttackableObject enemyObject = enemy.gameObject.GetComponentInParent<AttackableObject>();
            if (enemyObject.health > _lowestHealth)
            {
                SetTarget(enemy);
                _lowestHealth = enemyObject.health;
            }
        }
    }
    
    /// <summary>
    /// Finds Targets with the lowest Health.
    /// </summary>
    /// <param name="enemies">list of enemies in sight</param>
    private void FindEnemyWithLowestHealth(GameObject[] enemies)
    {
        target = null;
        float _lowestHealth = Mathf.Infinity;
        foreach (var enemy in enemies)
        {
            AttackableObject enemyObject = enemy.gameObject.GetComponentInParent<AttackableObject>();
            if (enemyObject.health < _lowestHealth)
            {
                SetTarget(enemy);
                _lowestHealth = enemyObject.health;
            }
        }
    }
    
    /// <summary>
    /// Follow first Target.
    /// </summary>
    /// <param name="enemies">list of enemies in sight</param>
    private void FollowFirstEnemy(GameObject[] enemies)
    {
        if (CurrentTargetStillInRange())
        {
            return;
        }

        target = null;
        GameObject nearestEnemy = FindNearestEnemyInRange(enemies);
        SetTarget(nearestEnemy);
    }
    
    /// <summary>
    /// Checks which given target is the nearest one.
    /// </summary>
    /// <param name="enemies">list of enemies in sight</param>
    /// <returns>nearest enemy</returns>
    private GameObject FindNearestEnemyInRange(GameObject[] enemies)
    {
        float _shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;
        foreach (var enemy in enemies)
        {
            Vector3 offset = enemy.transform.position - transform.position;
            offset.y = 0;
            float sqrLen = offset.sqrMagnitude;
            if (_rangeSquared < (sqrLen))
            {
                continue;
            }

            if (sqrLen < (_shortestDistance * _shortestDistance) && sqrLen < _rangeSquared)
            {
                nearestEnemy = enemy;
                _shortestDistance = Mathf.Sqrt(sqrLen);
            }
        }

        return nearestEnemy;
    }
    
    /// <summary>
    /// Changes the mode of the target finder.
    /// </summary>
    /// <param name="mode">new mode of target finder</param>
    public void ChangeMode(TargetFinderMode mode)
    {
        _mode = mode;
    }
    
    /// <summary>
    /// Returns the mode of the target finder.
    /// </summary>
    /// <returns>mode of the target finder</returns>
    public TargetFinderMode GetMode()
    {
        return _mode;
    }
    
    /// <summary>
    /// Sets the target of the target finder.
    /// </summary>
    /// <param name="setTarget">target to set</param>
    private void SetTarget(GameObject setTarget)
    {
        if (setTarget != null)
        {
            MeshRenderer r = setTarget.GetComponentInChildren<MeshRenderer>();
            target = r.gameObject;
        }
    }
}