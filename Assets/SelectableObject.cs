using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableObject : MonoBehaviour
{

    [SerializeField] private Material selectedMaterial;
    private ObjectMaterialController _omc;
    
    void Awake()
    {
        _omc = gameObject.GetComponent<ObjectMaterialController>();
    }

    public void IsSelected()
    {
        _omc.ChangeMaterial(selectedMaterial);
    }

    public void IsUnselected()
    {
        _omc.ReverToBaseMaterial();
    }
    
}
