using System;
using System.Collections;
using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

public class ProjectileParabolic : Projectile
{
    
    private Transform _target;
    public float speed;
    public event Action<int> OnHit = delegate { };
    private Rigidbody _rigidbody;
    
    public override void Launch()
    {
        _rigidbody = GetComponent<Rigidbody>();
        Vector3 direction = CalculateLaunchDirection();
        direction.y = 15;
        speed = CalculateSpeed(direction);
        Debug.Log(speed);
        _rigidbody.AddForce(direction * speed * 3.6f, ForceMode.Acceleration);
    }

    private float CalculateSpeed(Vector3 direction)
    {
        direction.y = 0;
        double v = Math.Sqrt((direction.magnitude * 9.81f) / 0.99827f);
        return (float) v;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction = CalculateLaunchDirection();
        float travelDinstanceInThisFrame = speed * Time.deltaTime;

        if (direction.magnitude <= travelDinstanceInThisFrame)
        {
            Destroy(gameObject);
            return;
        }
        
        //transform.Translate(direction.normalized * travelDinstanceInThisFrame, Space.World);

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
