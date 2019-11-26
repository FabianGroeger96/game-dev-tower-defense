using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using UnityEditor;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class Projectile : MonoBehaviour {
    
    // Start is called before the first frame update
    
    private Transform _target;
    public float speed;
    public event Action<int> OnHit = delegate { };
    private Rigidbody _rigidbody;
     
    
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        Vector3 direction = CalculateLaunchDirection();
        _rigidbody.AddForce(direction * speed, ForceMode.VelocityChange);
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

    public void Seek(Transform target)
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
