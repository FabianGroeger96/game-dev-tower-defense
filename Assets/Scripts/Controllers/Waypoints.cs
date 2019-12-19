using System;
using UnityEngine;

/// <summary>
/// Represents the way points of the game path.
/// </summary>
public class Waypoints : MonoBehaviour
{
    // static reference to the way points
    public static Transform[] waypoints;

    // reference to the way points defined in unity
    [SerializeField] private Transform[] _waypoints;

    /// <summary>
    /// Awake is being used to initialize all the reference the class needs,
    /// and to bring it to an initial state.
    /// </summary>
    private void Awake()
    {
        waypoints = _waypoints;
    }
}