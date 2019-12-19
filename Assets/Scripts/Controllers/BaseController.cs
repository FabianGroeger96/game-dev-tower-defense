using UnityEngine;

/// <summary>
/// Represents the base controller from which a lot of controllers inherit to get a reference to the GameManager.
/// </summary>
public class BaseController : MonoBehaviour
{
    // reference to the GameManager
    protected GameManager _gc;

    /// <summary>
    /// Initializes the GameManager.
    /// </summary>
    protected void Init()
    {
        _gc = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
}