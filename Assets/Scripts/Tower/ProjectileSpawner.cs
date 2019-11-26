using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    private Projectile _projectile;
    void Start()
    {
        _projectile = (Projectile) AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Projectile.prefab", typeof(Projectile));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void launch(Tower tower, Transform target, Quaternion rotation)
    {
        Projectile clone = Instantiate(_projectile, transform.position, rotation);
        clone.Seek(target);
        clone.OnHit += tower.HandleHit;
    }
    
}
