using System.Collections;
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
        // Get the directional vector 
        var dir = Vector3.Scale(new Vector3(1, 0, 1), _target.position - transform.position);
        transform.Translate(Time.deltaTime * speed * dir.normalized, Space.World);

        // Convert 3d positions to 2d vector
        var position2d = new Vector2(transform.position.x, transform.position.z);
        var target2d = new Vector2(_target.transform.position.x, _target.transform.position.z);

        if (Vector2.Distance(position2d, target2d) <= 0.1f)
        {
            if (_waypointIndex >= Waypoints.waypoints.Length)
            {
                Destroy(this.gameObject);
                // TODO - Remove health
                return;
            }

            NextWaypoint();
        }
    }

    private void NextWaypoint()
    {
        _waypointIndex++;
        _target = Waypoints.waypoints[_waypointIndex];
    }
}