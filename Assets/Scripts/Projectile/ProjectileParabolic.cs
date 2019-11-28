using System;
using System.Collections;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class ProjectileParabolic : Projectile
{
    
    private Transform _target;
    private float speed;
    public event Action<int> OnHit = delegate { };
    private Rigidbody _rigidbody;
    
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
        Debug.Log(direction.magnitude);
        double v = Math.Sqrt((direction.magnitude * 9.81f) / Math.Sin(1.5708));
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
