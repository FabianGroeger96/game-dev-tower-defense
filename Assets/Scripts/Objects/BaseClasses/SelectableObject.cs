using UnityEngine;

/// <summary>
/// Represents an object within the game, which can be selected.
/// </summary>
public class SelectableObject : MonoBehaviour
{
    // the material to display when the object is selected
    [SerializeField] private Material selectedMaterial;

    // reference to the controller to change the material
    private ObjectMaterialController _omc;

    /// <summary>
    /// Awake is being used to initialize all the reference the class needs,
    /// and to bring it to an initial state.
    /// </summary>
    void Awake()
    {
        _omc = gameObject.GetComponent<ObjectMaterialController>();
    }

    /// <summary>
    /// Will be called when the object is selected, it changes the material of the object to the selected one.
    /// </summary>
    public void IsSelected()
    {
        _omc.ChangeMaterial(selectedMaterial);
    }

    /// <summary>
    /// Will be called when the object is unselected, it changes the material to the default one.
    /// </summary>
    public void IsUnselected()
    {
        _omc.RevertToBaseMaterial();
    }
}