using UnityEngine;

/// <summary>
/// Represents a wave of the game.
/// </summary>
[System.Serializable]
public class Wave
{
    // the enemy of the current wave
    public GameObject enemy;

    // how many enemies in the current wave will be
    public int count;

    // how fast the enemies spawn
    public float rate;

    // the level of the wave
    public int level;
}