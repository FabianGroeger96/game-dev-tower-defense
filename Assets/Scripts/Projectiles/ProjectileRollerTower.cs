using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class ProjectileRollerTower : Projectile {
    
    private Rigidbody _rigidbody;
    public GameObject damageEffect;
     
    // Update is called once per frame
    public override void Launch()
    {
        _rigidbody = GetComponent<Rigidbody>();
        var direction = CalculateLaunchDirection();
        _rigidbody.AddForce(direction * properties["speed"], ForceMode.VelocityChange);
    }

    private Vector3 CalculateLaunchDirection()
    {
        var direction = target.position - transform.position;
        return direction;
    }
   
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
        } else if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
        {
            Destroy(gameObject);
        }
    }
}
