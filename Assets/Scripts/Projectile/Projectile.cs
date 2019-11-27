using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.WSA;

public abstract class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    
    public event Action<int> OnHit = delegate { };
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    abstract public void Launch();
    abstract public void Seek(Transform transform);
    
}
