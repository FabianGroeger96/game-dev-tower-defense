using UnityEngine;

/// <summary>
/// Represents the splash the parabolic tower does when the projectile hits the ground.
/// </summary>
public class Splash : MonoBehaviour
{
    // specifies the damage per particle
    public float damagePerParticle;

    // tracks how many enemies have been hit
    public int hits;

    /// <summary>
    /// Event when the projectile collides with an object.
    /// </summary>
    /// <param name="other">object the projectile collides with</param>
    void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            hits++;
            Enemy enemy = other.GetComponentInParent<Enemy>();
            enemy.DealDamage(damagePerParticle);
        }
    }
}