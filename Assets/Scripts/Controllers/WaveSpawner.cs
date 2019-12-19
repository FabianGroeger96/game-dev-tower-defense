using System.Collections;
using UnityEngine;

/// <summary>
/// Represents the wave spawner of the game, which spawns the current wave.
/// </summary>
public class WaveSpawner : MonoBehaviour
{
    // reference to the point where the wave will be spawned
    public Transform spawnPoint;

    /// <summary>
    /// Spawns a given wave.
    /// </summary>
    /// <param name="wave">given wave to spawn</param>
    /// <returns>NONE</returns>
    public IEnumerator SpawnWave(Wave wave, GameManager.GameMode mode)
    {
        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave, mode);
            yield return new WaitForSeconds(1f / wave.rate);
        }
    }

    /// <summary>
    /// Spawns an enemy of the current wave.
    /// </summary>
    /// <param name="wave">current wave</param>
    private void SpawnEnemy(Wave wave, GameManager.GameMode mode)
    {
        GameManager.enemiesAlive++;
        GameObject enemy = Instantiate(wave.enemy, spawnPoint.position, spawnPoint.rotation);
        Enemy enemyObject = enemy.GetComponent<Enemy>();
        // sets the level of the wave to the level of the enemy
        enemyObject.SetLevel(wave.level + (int) mode);
    }
}