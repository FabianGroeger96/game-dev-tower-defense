using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{

    private GameObject[] _enemies = new GameObject[10];
    private float _lastSpawn = Mathf.Infinity;
    private float _spawnRate = 2f;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _enemies[0] = (GameObject) AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Enemy.prefab", typeof(GameObject));
    }

    // Update is called once per frame
    void Update()
    {
        if (_lastSpawn > _spawnRate)
        {
            Instantiate(_enemies[0], transform.position, Quaternion.identity);
            _lastSpawn = 0;
        }
        else
        {
            _lastSpawn += Time.deltaTime;
        }
    }
}
