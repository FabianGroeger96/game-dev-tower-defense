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
        _angleInRad = DegreeToRad(properties["angle"]);
        Vector3 direction = CalculateLaunchDirection();
        direction = CalculateYComponent(direction.x, direction.z);
        float distance = CalculateDistance();
        float speed = CalculateSpeed(Mathf.Abs(distance));
        _rigidbody.AddForce(direction.normalized * speed, ForceMode.VelocityChange);
    }
    
    private float CalculateDistance()
    {
        return Mathf.Sqrt(
            Mathf.Pow(target.transform.position.x - transform.position.x, 2) + 
            Mathf.Pow(target.transform.position.z - transform.position.z, 2));
    }

    private Vector3 CalculateYComponent(float x, float z)
    {
        Vector4 vec = new Vector4(x, z, 0.0f, 1);
        float slope = Mathf.Abs(z / x);
        Matrix4x4 m_z = RotateZ(Mathf.Atan(slope));
        Vector4 on_axis = m_z * vec;
        Matrix4x4 m_y = RotateY(_angleInRad);
        on_axis = m_y * on_axis;
        on_axis = m_z.inverse * on_axis;
        return new Vector3(on_axis.x, Mathf.Abs(on_axis.z), on_axis.y);
    }

    private float CalculateSpeed(float distance)
    {
        double v = Mathf.Sqrt((distance * 9.81f) / Mathf.Sin(2 * _angleInRad));
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
