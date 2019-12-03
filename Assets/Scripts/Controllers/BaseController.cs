using UnityEngine;

public class BaseController : MonoBehaviour
{
    protected GameController _gc;
    public PlacementController placementController;

    public void Init()
    {
        _gc = GameObject.Find("GameController").GetComponent<GameController>();
    }
}