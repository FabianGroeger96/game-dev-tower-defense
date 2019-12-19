using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controller which controlls the state of the UI tower placement buttons.
/// </summary>
public class TowerPlacementButtonController : BaseController
{
    // reference to the placement button
    public Button placementButton;

    // reference to the placement buttons image, to change the background
    private Image _placementButtonImage;

    // reference to the placement buttons text, to change the text
    private Text _placementButtonText;

    // reference to the tower game object, which can placed by pressing the button
    public GameObject tower;

    // reference to the tower converted from the game object
    private Tower _tower;

    /// <summary>
    /// Awake is being used to initialize all the reference the class needs,
    /// and to bring it to an initial state.
    /// </summary>
    private void Awake()
    {
        Init();

        _tower = tower.GetComponent<Tower>();

        _placementButtonImage = placementButton.GetComponent<Image>();
        _placementButtonText = placementButton.gameObject.GetComponentInChildren<Text>();
        _placementButtonText.text = _tower.name + "\n" + "($" + _tower.costs + ")";
    }

    /// <summary>
    /// Within every frame the controller checks if there is enough money to place the tower of the button.
    /// If not, it disables it and the button gets grayed out.
    /// </summary>
    private void Update()
    {
        if (_gc.moneyCount >= _tower.costs)
        {
            placementButton.interactable = true;
        }
        else
        {
            placementButton.interactable = false;
            _placementButtonImage.color = Color.gray;
        }
    }

    /// <summary>
    /// Place the Tower on the map.
    /// </summary>
    /// <param name="towerId">tower to place</param>
    public void PlaceTower(int towerId)
    {
        _gc.SetToPlacementMode(towerId);
    }
}