using UnityEngine;

public class BaseController : MonoBehaviour
{
    public GameController gameController;
    public PlacementController placementController;

    public void Init()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
    }
}