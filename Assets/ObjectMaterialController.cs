using UnityEngine;

public class ObjectMaterialController : MonoBehaviour
{
    
    private MeshRenderer[] _renderers;
    private Material _previousMaterial;
    private Material _currentMaterial;
    private Material _baseMaterial;

    private void Awake()
    {
        _renderers = GetComponentsInChildren<MeshRenderer>();
    }

    public void SetBaseMaterial(Material baseMaterial)
    {
        _baseMaterial = baseMaterial;
    }
    
    public void ChangeMaterial(Material material)
    {
        _previousMaterial = _currentMaterial;
        _currentMaterial = material;
        foreach(MeshRenderer r in _renderers)
        {
            r.material = _currentMaterial;
        }
    }

    public void ReverToBaseMaterial()
    {
        ChangeMaterial(_baseMaterial);
    }

    public void RevertToPreviousMaterial()
    {
        if (_previousMaterial != null)
        {
            ChangeMaterial(_previousMaterial);
        }
        else
        {
            ChangeMaterial(_baseMaterial);
        }
    }
    
    
}
