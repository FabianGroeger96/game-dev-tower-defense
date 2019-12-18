using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public string[] eligableTargetTags;

    // Start is called before the first frame update
    public Color gizmoColor = Color.yellow;

    void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, range);
    }

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

    private void Awake()
    {
        _mode = TargetFinderMode.NearestEnemy;
        _rangeSquared = range * range;
        InvokeRepeating("UpdateTarget", 0f, 0.25f);
    }

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

    private void SetTarget(GameObject setTarget)
    {
        if (setTarget != null)
        {
            MeshRenderer r = setTarget.GetComponentInChildren<MeshRenderer>();
            target = r.gameObject;
        }
    }
}