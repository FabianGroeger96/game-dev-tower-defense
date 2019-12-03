﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Numerics;
using UnityEditor;
using UnityEngine;
using UnityEngine.WSA;
using Vector3 = UnityEngine.Vector3;

public class ProjectileStraight : Projectile {
    
    public int damage;
    private Rigidbody _rigidbody;
     
    // Update is called once per frame
    public override void Launch()
    {
        _rigidbody = GetComponent<Rigidbody>();
        Vector3 direction = CalculateLaunchDirection();
        _rigidbody.AddForce(direction * properties["speed"], ForceMode.VelocityChange);
    }

    private Vector3 CalculateLaunchDirection()
    {
        Vector3 direction = target.position - transform.position;
        return direction;
    }
   
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Enemy enemy = other.gameObject.GetComponentInParent<Enemy>();
            enemy.DealDamage(damage);
            Destroy(gameObject);
        }
    }
    
}
