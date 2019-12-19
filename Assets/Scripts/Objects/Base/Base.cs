using UnityEngine;

/// <summary>
/// Represents the "home" base of the game, which the enemies try to destroy.
/// </summary>
public class Base : AttackableObject
{
    // the effect it plays when the base is destroyed
    public GameObject damageEffect;

    // reference to the GameManager, to change the state of the game
    private GameManager _gc;

    // if the base has been killed yet
    private bool _killed = false;

    /// <summary>
    /// Awake is being used to initialize all the reference the class needs,
    /// and to bring it to an initial state.
    /// </summary>
    private void Awake()
    {
        health = initialHealth;
        _gc = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    /// <summary>
    /// Deals damage to the base, and kills it if the health is zero.
    /// </summary>
    /// <param name="damage">Damage to subtract from the base</param>
    public new void DealDamage(float damage)
    {
        // play damage effect
        var effect = Instantiate(damageEffect, transform.position, Quaternion.identity);
        Destroy(effect, 6f);
        // call base deal damage method
        SoundController.PlayBaseHit();
        SoundController.SpeedUpPaceOfMusic();
        base.DealDamage(damage);
    }

    /// <summary>
    /// Will be called when the health of the base reaches zero, it specifies what to do when the base is killed.
    /// </summary>
    protected override void Die()
    {
        // check if base isn't killed yet
        if (!_killed)
        {
            _killed = true;
            // play death effect
            var effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
            Destroy(effect, 6f);
            // destroy base
            Destroy(gameObject);
            // set game state to game over
            _gc.gameState = GameManager.GameState.GameOver;
        }
    }
}