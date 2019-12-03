using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


public class ProjectileSpawner : MonoBehaviour
{
    // Start is called before the first frame update
    public Projectile _projectile;
    
    
    public void launch(Tower tower, Transform target)
    {
        Projectile clone = Instantiate(_projectile, transform.position, Quaternion.identity);
        clone.Seek(target);
        clone.Launch();
        clone.OnHit += tower.HandleHit;
    }
    
}
