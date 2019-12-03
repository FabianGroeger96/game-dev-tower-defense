using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class ProjectileParabolic : Projectile
{

    private float _angleInRad;
    private Rigidbody _rigidbody;
    private Splash _particleSystem;
    
    void Start()
    {
        _particleSystem = (Splash) AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Towers/Silo/Splash.prefab", typeof(Splash));
    }
    
    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }
    }

    private float DegreeToRad(float degree)
    {
        return (float) (degree * (Math.PI / 180)); 
    }
    
    public override void Launch() 
    {
        _rigidbody = GetComponent<Rigidbody>();
        _angleInRad = DegreeToRad(properties["angle"]);
        Vector3 direction = CalculateLaunchDirection();
        direction.y = CalculateYComponent(direction.x, direction.z);
        float speed = CalculateSpeed(direction);
        _rigidbody.AddForce(direction.normalized * speed, ForceMode.Impulse);
    }

    private float CalculateYComponent(float x, float z)
    {
        return (float) Math.Sqrt((x * x) + (z * z) - (2 * x * z * Math.Cos(_angleInRad)));
    }

    private float CalculateSpeed(Vector3 direction)
    {
        direction.y = 0;
        double v = Math.Sqrt((direction.magnitude * 9.81f) / Math.Sin(2 * _angleInRad));
        return (float) v;
    }


    private Vector3 CalculateLaunchDirection()
    {
        Vector3 direction = target.position - transform.position;
        return direction;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        _particleSystem = (Splash) AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Towers/Silo/Splash.prefab", typeof(Splash));
        Instantiate(_particleSystem, transform.position, Quaternion.Euler(270f, 0f, 0f));
        if (other.gameObject.CompareTag("Enemy"))
        {
            //
        }


    }
    
}
