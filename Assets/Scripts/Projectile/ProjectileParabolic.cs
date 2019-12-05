using System;
using System.Collections;
using System.Numerics;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using Matrix4x4 = UnityEngine.Matrix4x4;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class ProjectileParabolic : Projectile
{

    private float _angleInRad;
    private Rigidbody _rigidbody;
    private Splash _splash;
    
  void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
        }
    }

    private float DegreeToRad(float degree)
    {
        return (float) (degree * (Math.PI / 180)); 
    }
    
    public override void Launch() 
    {
        _splash = (Splash) AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Towers/Silo/Splash.prefab", typeof(Splash));
        _rigidbody = GetComponent<Rigidbody>();
        _angleInRad = DegreeToRad(properties["angle"]);
        Vector3 direction = CalculateLaunchDirection();
        Debug.Log("Launch direction: " + direction.ToString());
        direction.y = CalculateYComponent(direction.x, direction.z);
        Debug.Log("Calculate Y, with angle: " + direction.ToString());
        float distance = CalculateDistance();
        Debug.Log("magnitude " + direction.magnitude.ToString());
        Debug.Log("distance " + distance);
        float speed = CalculateSpeed(distance);
        Debug.Log("Launch direction: " + direction.normalized.ToString());
        Debug.Log(speed);
        _rigidbody.AddForce(direction * speed, ForceMode.Impulse);
    }

    private Vector3 CalculateVectorWithAngle(Vector3 direction)
    {
        return new Vector3(0f, 0f, 0f);
    }

    private float CalculateDistance()
    {
        return Mathf.Sqrt(
            Mathf.Abs(
                Mathf.Pow(target.transform.position.x - transform.position.x, 2)) +
            Mathf.Abs(
                Mathf.Pow(target.transform.position.z - transform.position.z, 2)));
    }

    private float CalculateYComponent(float x, float z)
    {
        return (float) Mathf.Sqrt((x * x) + (z * z) - (2 * x * z * Mathf.Cos(_angleInRad)));
    }

    private float CalculateSpeed(float distance)
    {
        double v = Mathf.Sqrt((distance * 9.81f) / Mathf.Sin(2 * _angleInRad));
        return (float) v;
    }


    private Vector3 CalculateLaunchDirection()
    {
        Vector3 direction = target.position - transform.position;
        direction.y = 0.25f;
        return direction;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        _splash = (Splash) AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Towers/Silo/Splash.prefab", typeof(Splash));
        Instantiate(_splash, transform.position, Quaternion.Euler(270f, 0f, 0f));
        if (other.gameObject.CompareTag("Enemy"))
        {
            //
        }


    }
    
}
