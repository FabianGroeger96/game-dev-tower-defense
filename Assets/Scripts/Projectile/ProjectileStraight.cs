using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using UnityEditor;
using UnityEngine;
using UnityEngine.WSA;
using Vector3 = UnityEngine.Vector3;

public class ProjectileStraight : Projectile {
    
    // Start is called before the first frame update
    
    private Transform _target;
    public float speed;
    public int damage;
    private Rigidbody _rigidbody;
     
    // Update is called once per frame
    public override void Launch()
    {
        _rigidbody = GetComponent<Rigidbody>();
        Vector3 direction = CalculateLaunchDirection();
        _rigidbody.AddForce(direction.normalized * speed, ForceMode.VelocityChange);
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
            Debug.Log("taged enemy hit");
            Enemy enemy = other.gameObject.GetComponentInParent<Enemy>();
            enemy.DealDamage(damage);
            Destroy(gameObject);
        }
    }
    
    IEnumerator TestCoroutine()
    {
        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }
    
}
