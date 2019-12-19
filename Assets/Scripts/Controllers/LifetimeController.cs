using UnityEngine;

/// <summary>
/// Used to give an object a time to live and after then delete it.
/// </summary>
public class LifetimeController : MonoBehaviour
{
    // time the object has to live
    public float timeToLive = 3f;

    /// <summary>
    /// When the object is started, it destroys it after the life time.
    /// </summary>
    private void Start()
    {
        Destroy(gameObject, timeToLive);
    }
}