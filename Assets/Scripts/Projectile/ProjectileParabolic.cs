using System;
using System.Collections;
using UnityEditor;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class ProjectileParabolic : Projectile
{
    
    private Transform _target;
    private float speed;
    public event Action<int> OnHit = delegate { };
    private Rigidbody _rigidbody;
    private Splash _particleSystem;
    
    
    void Start()
    {
        _particleSystem = (Splash) AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Towers/Silo/Splash.prefab", typeof(Splash));
    }
    
    void Update()
    {
        if (_target == null)
        {
            Destroy(gameObject);
            return;
        }
    }
    
    public override void Launch() 
    {
        _rigidbody = GetComponent<Rigidbody>();
        Vector3 direction = CalculateLaunchDirection();
        direction.y = CalculateYComponent(direction.x, direction.z);
        speed = CalculateSpeed(direction);
        _rigidbody.AddForce(direction.normalized * speed, ForceMode.Impulse);
    }

    private float CalculateYComponent(float x, float z)
    {
        return (float) Math.Sqrt((x * x) + (z * z) - (2 * x * z * Math.Cos(0.785398)));
    }

    private float CalculateSpeed(Vector3 direction)
    {
        direction.y = 0;
        double v = Math.Sqrt((direction.magnitude * 9.81f) / Math.Sin(1.5708));
        return (float) v;
    }


    private Vector3 CalculateLaunchDirection()
    {
        Vector3 direction = _target.position - transform.position;
        return direction;
    }

    public override void Seek(Transform target)
    {
        _target = target;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.layer);
        _particleSystem = (Splash) AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Towers/Silo/Splash.prefab", typeof(Splash));
        Instantiate(_particleSystem, transform.position, Quaternion.Euler(270f, 0f, 0f));
        if (other.gameObject.CompareTag("Enemy"))
        {
            //StartCoroutine(TestCoroutine());
            Destroy(gameObject);
            OnHit(1);
        }


    }
    
    IEnumerator TestCoroutine()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
    
}
