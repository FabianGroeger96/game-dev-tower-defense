using UnityEngine;
using Vector3 = UnityEngine.Vector3;

/// <summary>
/// Represents the projectile of the roller tower.
/// It inherits from the projectile.
/// </summary>
public class ProjectileRollerTower : Projectile
{
    // the effect which will play when the projectile hits a target
    public GameObject damageEffect;

    // reference to the projectiles rigid body
    private Rigidbody _rigidbody;

    /// <summary>
    /// Launches the projectile of the roller tower.
    /// </summary>
    public override void Launch()
    {
        _rigidbody = GetComponent<Rigidbody>();
        var direction = CalculateLaunchDirection();
        _rigidbody.AddForce(direction * properties["speed"], ForceMode.VelocityChange);
    }

    /// <summary>
    /// Calculates the launch direction of the projectile.
    /// </summary>
    /// <returns>the launch direction of the projectile</returns>
    private Vector3 CalculateLaunchDirection()
    {
        var direction = target.position - transform.position;
        return direction;
    }

    /// <summary>
    /// Event when the projectile collides with an object.
    /// </summary>
    /// <param name="other">object the projectile collides with</param>
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            // play the damage effect of the bullet
            var effect = Instantiate(damageEffect, transform.position, Quaternion.identity);
            Destroy(effect, 3f);

            var enemy = other.gameObject.GetComponentInParent<Enemy>();
            enemy.DealDamage(damage);
            Destroy(gameObject);
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}