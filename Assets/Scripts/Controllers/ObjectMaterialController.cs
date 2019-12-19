using UnityEngine;

/// <summary>
/// Controller to change the materials of an object.
/// </summary>
public class ObjectMaterialController : MonoBehaviour
{
    // reference to the mesh renderers
    private MeshRenderer[] _renderers;

    // reference to the current material
    private Material _currentMaterial;

    // reference to the base material
    private Material _baseMaterial;

    /// <summary>
    /// Awake is being used to initialize all the reference the class needs,
    /// and to bring it to an initial state.
    /// </summary>
    private void Awake()
    {
        _renderers = GetComponentsInChildren<MeshRenderer>();
    }

    /// <summary>
    /// Sets the base material to the given one.
    /// </summary>
    /// <param name="baseMaterial">base material</param>
    public void SetBaseMaterial(Material baseMaterial)
    {
        _baseMaterial = baseMaterial;
    }

    /// <summary>
    /// Changes the material of an object to the given one.
    /// </summary>
    /// <param name="material">material to change to</param>
    public void ChangeMaterial(Material material)
    {
        _currentMaterial = material;
        foreach (MeshRenderer r in _renderers)
        {
            r.material = _currentMaterial;
        }
    }

    /// <summary>
    /// Reverses the object to the base material.
    /// </summary>
    public void RevertToBaseMaterial()
    {
        ChangeMaterial(_baseMaterial);
    }
}