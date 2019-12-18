using System;
using System.Collections;
using UnityEngine;
using Matrix4x4 = UnityEngine.Matrix4x4;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;
using Vector4 = UnityEngine.Vector4;

public class ProjectileParabolic : Projectile
{

    private float _angleInRad;
    private Rigidbody _rigidbody;
    [SerializeField] Splash _splash;
    
    private float DegreeToRad(float degree)
    {
        return (float) (degree * (Math.PI / 180)); 
    }
    
    public static Matrix4x4 RotateZ(float aAngleRad)
    {
        Matrix4x4 m = Matrix4x4.identity;     // cos -sin 0   0
        m.m00 = m.m11 = Mathf.Cos(aAngleRad); // sin  cos 0   0
        m.m10 = Mathf.Sin(aAngleRad);         //  0   0   1   0
        m.m01 = -m.m10;                       //  0   0   0   1
        return m;
    }
    
    public static Matrix4x4 RotateY(float aAngleRad)
    {
        Matrix4x4 m = Matrix4x4.identity;     // cos  0  sin  0
        m.m00 = m.m22 = Mathf.Cos(aAngleRad); //  0   1   0   0
        m.m02 = Mathf.Sin(aAngleRad);         //-sin  0  cos  0
        m.m20 = -m.m02;                       //  0   0   0   1
        return m;
    }
    
    public override void Launch() 
    {
        _rigidbody = GetComponent<Rigidbody>();
        Vector3 direction = CalculateLaunchDirection();
        direction.y = CalculateYComponent(direction.x, direction.z);
        float speed = CalculateSpeed(direction);
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



    private Vector3 CalculateLaunchDirection()
    {
        Vector3 direction = target.position - transform.position;
        return direction;
    }

    
    private void OnCollisionEnter(Collision other)
    {
        Instantiate(_splash, transform.position, Quaternion.Euler(270f, 0f, 0f));
        if (other.gameObject.CompareTag("Ground"))
        {
            StartCoroutine(Wait());
        }
    }
    
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}
