using System;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
    public static Transform[] waypoints;
    [SerializeField] private Transform[] _waypoints;

    private void Awake()
    {
        waypoints = _waypoints;
    }
}