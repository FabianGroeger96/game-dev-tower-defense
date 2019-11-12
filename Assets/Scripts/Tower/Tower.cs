using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.XR;


public class Tower : MonoBehaviour
{
    [SerializeField] 
    public int exp;
    private ProjectileSpawner _spawner;
    private TargetFinder _targetFinder;
    // Start is called before the first frame update
    
    void Start()
    {
        _spawner = GetComponentInChildren<ProjectileSpawner>();
        _targetFinder = GetComponentInChildren<TargetFinder>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _spawner.launch(this, _targetFinder.target.transform);
        }
    }

    public void HandleHit(int obj)
    {
        exp += obj;
    }
    
}
