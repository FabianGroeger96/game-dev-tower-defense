using System;
using System.Collections;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

/// <summary>
/// Represents the projectile of the parabolic tower.
/// It inherits from the projectile.
/// </summary>
public class ProjectileParabolicTower : Projectile
{
    // the splash effect to play when a projectile hits the ground
    [SerializeField] Splash _splash;
    
    // angle of the shoot
    [SerializeField] private float _angle;

    // reference to the projectiles rigid body
    private Rigidbody _rigidbody;
    
    /// <summary>
    /// Launches the parabolic projectile.
    /// </summary>
    public override void Launch()
    {
        _rigidbody = GetComponent<Rigidbody>();
        Vector3 direction = new Vector3(0f, 1f, 0f);
        if (_angle > 45f)
        {
            speed = 5f;
        }
        else
        {
            direction = CalculateLaunchDirection();
            direction.y = CalculateYComponent(direction.x, direction.z);
            speed = CalculateSpeed(direction);
        }
        _rigidbody.AddForce(direction.normalized * speed, ForceMode.Impulse);
    }

    /// <summary>
    /// Calculates the Y component based on the X and Y one to fly in a curve.
    /// </summary>
    /// <param name="x">X component</param>
    /// <param name="z">Y component</param>
    /// <returns></returns>
    private float CalculateYComponent(float x, float z)
    {
        return (float) Math.Sqrt((x * x) + (z * z) - (2 * x * z * Math.Cos(_angle * Mathf.Deg2Rad)));
    }

    /// <summary>
    /// Calculates the speed of the projectile in a given direction.
    /// </summary>
    /// <param name="direction">in which direction the projectile has to move</param>
    /// <returns></returns>
    private float CalculateSpeed(Vector3 direction)
    {
        direction.y = 0;
        double v = Math.Sqrt((direction.magnitude * 9.81f) / Math.Sin(2 * _angle * Mathf.Deg2Rad));
        return (float) v;
    }

    /// <summary>
    /// Calculates the launch direction.
    /// </summary>
    /// <returns>Launch direction</returns>
    private Vector3 CalculateLaunchDirection()
    {
        Vector3 direction = target.position - transform.position;
        return direction;
    }

    /// <summary>
    /// Event when the projectile collides with the ground.
    /// </summary>
    /// <param name="other">object the projectile collides with</param>
    private void OnCollisionEnter(Collision other)
    {
        Instantiate(_splash, transform.position, Quaternion.Euler(270f, 0f, 0f));
        if (other.gameObject.CompareTag("Ground"))
        {
            StartCoroutine(Wait());
        }
    }

    /// <summary>
    /// Waits for 1f second and then destroys the projectile.
    /// </summary>
    /// <returns>NONE</returns>
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1f);
        Destroy(gameObject);
    }
}