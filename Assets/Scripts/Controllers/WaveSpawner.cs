using System.Collections;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public Transform spawnPoint;
    private Wave _currentWave;
    
    public IEnumerator SpawnWave(Wave wave)
    {
        _currentWave = wave;
        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(1f / wave.rate);
        }
    }

    void SpawnEnemy()
    {
        GameController.enemiesAlive++;
        GameObject enemy = Instantiate(_currentWave.enemy, spawnPoint.position, spawnPoint.rotation);
        Enemy enemyObject = enemy.GetComponent<Enemy>();
        enemyObject.SetLevel(_currentWave.level);
    }
}