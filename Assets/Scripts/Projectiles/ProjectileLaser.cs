using System.Collections;
using UnityEngine;

/// <summary>
/// Represents the projectile of the laser.
/// It inherits from the projectile.
/// </summary>
public class ProjectileLaser : Projectile
{
    // the effect it plays when the laser hits the target
    public GameObject damageEffect;

    // to render the line of the laser
    private LineRenderer _lr;

    // enemy to attack
    private Enemy _enemy;

    // if the laser has been launched yet
    private bool _launched;

    /// <summary>
    /// Changes the position of the line (laser) to follow the target after the position of the enemy was updated.
    /// </summary>
    void LateUpdate()
    {
        if (_launched && target != null)
        {
            _lr.SetPosition(1, target.position);
        }
    }

    /// <summary>
    /// Launches the laser projectile.
    /// </summary>
    public override void Launch()
    {
        _lr = GetComponent<LineRenderer>();
        _lr.SetPosition(0, transform.position);
        _lr.SetPosition(1, target.position);
        var effect = Instantiate(damageEffect, target.position, Quaternion.identity);
        Destroy(effect, 3f);
        _enemy = target.gameObject.GetComponentInParent<Enemy>();
        _launched = true;
        StartCoroutine(Wait());
    }

    /// <summary>
    /// Waits for 0.5f seconds and then deals damage to the target enemy and destroys the projectile after it.
    /// </summary>
    /// <returns>NONE</returns>
    IEnumerator Wait()
    {
        yield return new WaitForSeconds(0.5f);
        if (_enemy != null)
        {
            _enemy.DealDamage(damage);
        }

        _launched = false;
        Destroy(gameObject);
    }
}