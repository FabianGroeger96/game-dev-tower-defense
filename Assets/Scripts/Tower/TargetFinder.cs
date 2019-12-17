using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetFinder : MonoBehaviour
{
    public enum TargetFinderMode
    {
        NearestEnemy,
        FollowFirstEnemy,
        LowestHealth,
        HighestHealth
    };

    public GameObject target;

    public float range = 15f;
    private float _rangeSquared;
    private TargetFinderMode _mode;

    public string enemyTag;

    // Start is called before the first frame update
    public Color gizmoColor = Color.yellow;

    void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, range);
    }


    private void Start()
    {
        _mode = TargetFinderMode.HighestHealth;
        _rangeSquared = range * range;
        InvokeRepeating("UpdateTarget", 0f, 0.1f);
    }

    void UpdateTarget()
    {
        _rangeSquared = range * range;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        if (_mode == TargetFinderMode.NearestEnemy)
        {
            target = FindNearestEnemyInRange(enemies);
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
    
    private bool currentTargetStillInRange()
    {
        if (target != null)
        {
            Vector3 offset_to_current_target = target.transform.position - transform.position;
            offset_to_current_target.y = 0;
            float sqrLen_to_current_target = offset_to_current_target.sqrMagnitude;

            if (sqrLen_to_current_target < _rangeSquared)
            {
                return true;
            }
            return false;
        }
        return false;
    }
    
    private void FindEnemyWithHighestHealth(GameObject[] enemies)
    {
        target = null;
        float _lowestHealth = -Mathf.Infinity;
        foreach (var enemy in enemies)
        {
            AttackableObject enemyObject = enemy.gameObject.GetComponentInParent<AttackableObject>();
            if (enemyObject.health > _lowestHealth)
            {
                target = enemy;
                _lowestHealth = enemyObject.health;
            }
        }
    }


    private void FindEnemyWithLowestHealth(GameObject[] enemies)
    {
        target = null;
        float _lowestHealth = Mathf.Infinity;
        foreach (var enemy in enemies)
        {
            AttackableObject enemyObject = enemy.gameObject.GetComponentInParent<AttackableObject>();
            if (enemyObject.health < _lowestHealth)
            {
                target = enemy;
                _lowestHealth = enemyObject.health;
            }
        }
    }

    private void FollowFirstEnemy(GameObject[] enemies)
    {
        if (currentTargetStillInRange())
        {
            return;
        }

        target = null;
        GameObject nearestEnemy = FindNearestEnemyInRange(enemies);
        target = nearestEnemy;
    }

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

    public void ChangeMode(TargetFinderMode mode)
    {
        _mode = mode;
    }

    public TargetFinderMode GetMode()
    {
        return _mode;
    }
}
