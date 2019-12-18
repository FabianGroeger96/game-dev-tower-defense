using UnityEngine;

public class BaseController : MonoBehaviour
{
    protected GameManager _gc;

    protected void Init()
    {
        _gc = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
}