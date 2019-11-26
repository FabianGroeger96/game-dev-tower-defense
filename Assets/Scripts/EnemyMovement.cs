﻿using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class EnemyMovement : MonoBehaviour
{

    public float speed = 1f;
    private int _waypointIndex;
    private Transform _target;
    
    // Start is called before the first frame update
    void Start()
    {
        _waypointIndex = 0; 
        _target = Waypoints.waypoints[_waypointIndex];
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = Vector3.Scale(new Vector3(1,0,1), _target.position - transform.position);
        transform.Translate(dir.normalized*speed*Time.deltaTime, Space.World);
        Vector2 position2d = new Vector2(transform.position.x, transform.position.z);
        Vector2 target2d = new Vector2(_target.transform.position.x, _target.transform.position.z);
        if (Vector2.Distance(position2d, target2d) <= 0.1f)
        {
            if (_waypointIndex >= Waypoints.waypoints.Length)
            {
                Destroy(this.gameObject);
            }
            nextWaypoint();
        }
    }

    private void nextWaypoint()
    {
        _waypointIndex++;
        _target = Waypoints.waypoints[_waypointIndex];
    }
}
