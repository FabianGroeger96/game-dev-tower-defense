using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public Transform spawnPoint;

    public IEnumerator SpawnWave(Wave wave)
    {
        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave);
            yield return new WaitForSeconds(1f / wave.rate);
        }
    }

    private void SpawnEnemy(Wave wave)
    {
        GameManager.enemiesAlive++;
        GameObject enemy = Instantiate(wave.enemy, spawnPoint.position, spawnPoint.rotation);
        Enemy enemyObject = enemy.GetComponent<Enemy>();
        enemyObject.SetLevel(wave.level);
    }
}